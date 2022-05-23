using System.Security.Cryptography;
using Microsoft.AspNetCore.SignalR.Client;
using CryptoP2P.Backend.Messages;
using System.Text;

namespace CryptoP2P.Backend.Services;

public class ConnectionManager : IConnectionManager
{
  private HubConnection _peerConnection;

  private readonly ICryptoVault _cryptoVault;

  public ConnectionManager(ICryptoVault cryptoVault)
  {
    _cryptoVault = cryptoVault;
  }

  public async Task ConnectPeer(string url)
  {
    _peerConnection = new HubConnectionBuilder()
      .WithUrl(url)
      .Build();
    await _peerConnection.StartAsync();
    RegisterHandlers();
  }

  public async Task InvokeAsync(string endpoint, object? arg)
  {
    await _peerConnection.InvokeAsync(endpoint, arg);
  }

  public async Task InvokeAsync(string endpoint)
  {
    await _peerConnection.InvokeAsync(endpoint);
  }

  public void RegisterHandlers()
  {
    // Executed on initiator
    // Called by other server to initiator server
    // Everything is setup, passes session key encrypted via acquired public key
    _peerConnection.On<InitConversationMessage>("ServerServerInitResponse", async (message) => {
      _cryptoVault.SavePublicKey(message.InitiatorPublicKey);

      //  Send Session Key
      using var rsa = RSA.Create();
      rsa.ImportRSAPublicKey(_cryptoVault.LoadPublicKey(), out int _);

      var sessionKey = _cryptoVault.LoadSessionkey();
      var key = rsa.Encrypt(Encoding.UTF8.GetBytes(sessionKey.Key), RSAEncryptionPadding.Pkcs1);
      var iv = rsa.Encrypt(Encoding.UTF8.GetBytes(sessionKey.IV), RSAEncryptionPadding.Pkcs1);

      await _peerConnection.InvokeAsync("PassSessionKey", new PassSessionKeyMessage
      {
        Key = key,
        IV = iv
      });
    });
  }
}