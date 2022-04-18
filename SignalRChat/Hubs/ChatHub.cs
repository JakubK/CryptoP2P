using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs;

public class ChatHub : Hub
{
  public async Task SendMessage(string message)
  {
    await Clients.All.SendAsync("SendMessage", message);
  }

  public async Task InitConversation(string chatEndpoint)
  {
    await Clients.All.SendAsync("PeerRequest", chatEndpoint);
  }
}