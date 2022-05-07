using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using CryptoP2P.Backend.Messages;
using CryptoP2P.Backend.Models;
using CryptoP2P.Backend.Services;

namespace CryptoP2P.Backend.Hubs;

public class ChatHub : Hub
{
  private readonly IConnectionManager _connectionManager;
  private readonly ICryptoManager _cryptoManager;
  private readonly ICryptoVault _cryptoVault;

  public ChatHub(IConnectionManager connectionManager, ICryptoManager cryptoManager, ICryptoVault cryptoVault)
  {
    _connectionManager = connectionManager;
    _cryptoManager = cryptoManager;
    _cryptoVault = cryptoVault;
  }

  // Called on any peer wanting to send a message
  public async Task SendMessage(ChatMessage chatMessage)
  {
    //  Encrypt given message via session key
    var encryptedMessage = _cryptoManager.Encrypt(chatMessage.Message);
    await _connectionManager.InvokeAsync("ReceiveEncryptedMessage", new EncryptedChatMessage
    {
      EncryptedMessage = encryptedMessage,
      Type = chatMessage.Type,
      BlockMode = chatMessage.BlockMode
    });
  }

  public async Task ReceiveEncryptedMessage(EncryptedChatMessage encryptedChatMessage)
  {
    var message = _cryptoManager.Decrypt(encryptedChatMessage.EncryptedMessage);
    await Clients.All.SendAsync("ReceiveMessage", new ChatMessage
    {
      BlockMode = encryptedChatMessage.BlockMode,
      Message = message,
      Type = encryptedChatMessage.Type
    });
  }

  //  Method called by Frontend Client to connect with Peer
  //  Generated its RSA and passes to peer via ServerServerInit
  public async Task InitConversation(string chatEndpoint)
  {
    await _connectionManager.ConnectPeer(chatEndpoint);

    //  Get logged user RSA
    var myPublicKey = _cryptoVault.LoadMyPublicKey();
    
    await _connectionManager.InvokeAsync("ServerServerInit", new InitConversationMessage
    {
      InitiatorPublicKey = myPublicKey
    });
  }

  //  Executed on Peer
  //  Method called by Initiator Server to Peer Server
  //  Generates its rsa and passes public key
  public async Task ServerServerInit(InitConversationMessage message)
  {
    var feature = Context.Features.Get<IHttpConnectionFeature>();
    var endpoint = "http://" + feature.RemoteIpAddress.MapToIPv4() + "/chat";
    await _connectionManager.ConnectPeer(endpoint);

    _cryptoVault.SavePublicKey(message.InitiatorPublicKey);

    //  Get my public key and pass it to the initiator
    var myPublicKey = _cryptoVault.LoadMyPublicKey();

    await Clients.All.SendAsync("ServerServerInitResponse", new InitConversationMessage
    {
      InitiatorPublicKey = myPublicKey
    });
  }

  // Executed on peer
  // Decrypt everything via private key and respond that everything is ok
  // and conversation can be started
  public async Task PassSessionKey(PassSessionKeyMessage message)
  {
    using var rsa = RSA.Create();
    rsa.ImportRSAPrivateKey(_cryptoVault.LoadPrivateKey(), out var _);
    _cryptoVault.SaveSessionKey(
      new SessionKey()
      {
        Key = rsa.Decrypt(message.Key, RSAEncryptionPadding.Pkcs1),
        IV = rsa.Decrypt(message.IV, RSAEncryptionPadding.Pkcs1)
      }
    );
    await _connectionManager.InvokeAsync("PassSessionComplete");
    await Clients.All.SendAsync("ConversationStarted");
  }

  // Executed on Initiator
  // Calls Initiator to inform its frontend about starting safe conversation
  public async Task PassSessionComplete()
  {
    await Clients.All.SendAsync("ConversationStarted");
  }
}