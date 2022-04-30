namespace CryptoP2P.Backend.Messages;

public class PassSessionKeyMessage
{
  public byte[] Key { get; set; }
  public byte[] IV { get; set; }
}