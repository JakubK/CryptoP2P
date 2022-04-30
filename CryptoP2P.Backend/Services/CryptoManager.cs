using System.Security.Cryptography;
using System.Text;

namespace CryptoP2P.Backend.Services;

public class CryptoManager : ICryptoManager
{
  private readonly ICryptoVault _cryptoVault;
  public CryptoManager(ICryptoVault cryptoVault)
  {
    _cryptoVault = cryptoVault;
  }

  public string Decrypt(byte[] encryptedMessage)
  {
    using var aes = Aes.Create();
    var sessionKey = _cryptoVault.LoadSessionkey();
    aes.Key = sessionKey.Key;
    aes.IV = sessionKey.IV;
    return Encoding.UTF8.GetString(aes.DecryptCbc(encryptedMessage, aes.IV));
  }

  public byte[] Encrypt(string message)
  {
    using var aes = Aes.Create();
    var sessionKey = _cryptoVault.LoadSessionkey();
    aes.Key = sessionKey.Key;
    aes.IV = sessionKey.IV;
    return aes.EncryptCbc(Encoding.UTF8.GetBytes(message), aes.IV);
  }
}