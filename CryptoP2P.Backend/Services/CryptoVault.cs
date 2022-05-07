using CryptoP2P.Backend.Models;

namespace CryptoP2P.Backend.Services;

public class CryptoVault : ICryptoVault
{
  private byte[] _privateKey;
  private byte[] _publicKey;

  private byte[] _myPublicKey;

  private SessionKey _sessionKey;

  public byte[] LoadMyPublicKey() => _myPublicKey;
  public byte[] LoadPrivateKey() => _privateKey;
  public byte[] LoadPublicKey() => _publicKey;
  public SessionKey LoadSessionkey() => _sessionKey;

  public void SaveMyPublicKey(byte[] myPublicKey) => _myPublicKey = myPublicKey;
  public void SavePrivateKey(byte[] privateKey) => _privateKey = privateKey;
  public void SavePublicKey(byte[] publicKey) => _publicKey = publicKey;
  public void SaveSessionKey(SessionKey sessionKey) => _sessionKey = sessionKey;
}