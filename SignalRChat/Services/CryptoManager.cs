using System.Security.Cryptography;
using System.Text;

namespace SignalRChat.Services;

public class CryptoManager : ICryptoManager
{
  private byte[] _privateKey;
  private byte[] _publicKey;

  private byte[] _key;
  private byte[] _iv;

  public string Decrypt(byte[] encryptedMessage)
  {
    using var aes = Aes.Create();
    aes.Key = _key;
    aes.IV = _iv;
    return Encoding.UTF8.GetString(aes.DecryptCbc(encryptedMessage, _iv));
  }

  public byte[] Encrypt(string message)
  {
    using var aes = Aes.Create();
    aes.Key = _key;
    aes.IV = _iv;
    return aes.EncryptCbc(Encoding.UTF8.GetBytes(message), _iv);
  }

  public byte[] LoadPrivateKey() => _privateKey;

  public byte[] LoadPublicKey() => _publicKey;

  public (byte[], byte[]) LoadSessionkey() => (_key, _iv);

  public void StorePrivateKey(byte[] privateKey)
  {
    _privateKey = privateKey;
  }

  public void StorePublicKey(byte[] publicKey)
  {
    _publicKey = publicKey;
  }

  public void StoreSessionKey(byte[] key, byte[] iv)
  {
    _key = key;
    _iv = iv;
  }
}
