namespace CryptoP2P.Backend.Messages;

public class EncryptedChatMessage
{
  public string EncryptedMessage { get; set; }
  public string Type { get; set; }
  public string BlockMode { get; set; }
}