namespace SignalRChat.Models;

public class SessionKey
{
  public byte[] Key { get; set; }
  public byte[] IV { get; set; }
}