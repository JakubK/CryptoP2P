namespace CryptoP2P.Backend.Services;

public interface ICryptoManager
{
  byte[] Encrypt(string message);
  string Decrypt(byte[] encryptedMessage);
}