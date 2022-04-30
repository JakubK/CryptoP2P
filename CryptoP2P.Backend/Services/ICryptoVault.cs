using CryptoP2P.Backend.Models;

namespace CryptoP2P.Backend.Services;

public interface ICryptoVault
{
  void SavePrivateKey(byte[] privateKey);
  byte[] LoadPrivateKey();

  void SavePublicKey(byte[] publicKey);
  byte[] LoadPublicKey();

  void SaveSessionKey(SessionKey sessionKey);
  SessionKey LoadSessionkey();
}