namespace SignalRChat.Services;

public interface ICryptoManager
{
  void StorePrivateKey(byte[] privateKey);
  byte[] LoadPrivateKey();
  void StorePublicKey(byte[] publicKey);
  byte[] LoadPublicKey();
  void StoreSessionKey(byte[] key, byte[] iv);
  (byte[], byte[]) LoadSessionkey();

  byte[] Encrypt(string message);
  string Decrypt(byte[] encryptedMessage);
}