namespace CryptoP2P.Backend.Messages;

public class EncryptedChatMessage
{
  public byte[] EncryptedMessage { get; set; }
  public string Type { get; set; }
  public string BlockMode { get; set; }
}