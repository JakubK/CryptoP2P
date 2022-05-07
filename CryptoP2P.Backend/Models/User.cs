namespace CryptoP2P.Backend.Models;

public class User
{
  public string Id { get; set; }

  public byte[] PublicKey { get; set; }
  public byte[] EncryptedPrivateKey { get; set; }
  public byte[] IV { get; set; }

  public byte[] PasswordHash { get; set; }
}