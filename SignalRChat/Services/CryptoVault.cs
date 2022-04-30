using SignalRChat.Models;

namespace SignalRChat.Services;

public class CryptoVault : ICryptoVault
{
  private byte[] _privateKey;
  private byte[] _publicKey;

  private SessionKey _sessionKey;

  public byte[] LoadPrivateKey() => _privateKey;
  public byte[] LoadPublicKey() => _publicKey;
  public SessionKey LoadSessionkey() => _sessionKey;

  public void SavePrivateKey(byte[] privateKey) => _privateKey = privateKey;
  public void SavePublicKey(byte[] publicKey) => _publicKey = publicKey;
  public void SaveSessionKey(SessionKey sessionKey) => _sessionKey = sessionKey;
}