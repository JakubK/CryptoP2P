using System.Security.Cryptography;
using Microsoft.AspNetCore.SignalR.Client;
using CryptoP2P.Backend.Messages;
using CryptoP2P.Backend.Models;

namespace CryptoP2P.Backend.Services;

public class ConnectionManager : IConnectionManager
{
  private HubConnection _peerConnection;

  private readonly ICryptoManager _cryptoManager;
  private readonly ICryptoVault _cryptoVault;

  public ConnectionManager(ICryptoManager cryptoManager, ICryptoVault cryptoVault)
  {
    _cryptoManager = cryptoManager;
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
      //  Generate Session Key
      using var aes = Aes.Create();

      var key = aes.Key;
      var iv = aes.IV;

      _cryptoVault.SaveSessionKey(new SessionKey{
        Key = key,
        IV = iv
      });

      using var rsa = RSA.Create();
      rsa.ImportRSAPublicKey(_cryptoVault.LoadPublicKey(), out int _);
      key = rsa.Encrypt(key, RSAEncryptionPadding.Pkcs1);
      iv = rsa.Encrypt(iv, RSAEncryptionPadding.Pkcs1);
      await _peerConnection.InvokeAsync("PassSessionKey", new PassSessionKeyMessage
      {
        Key = key,
        IV = iv
      });
    });
  }
}