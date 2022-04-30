namespace SignalRChat.Services;

public interface ICryptoManager
{
  byte[] Encrypt(string message);
  string Decrypt(byte[] encryptedMessage);
}