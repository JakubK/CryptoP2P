using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs;

public class ChatHub : Hub
{
  public async Task SendMessage(string message)
  {
    await Clients.All.SendAsync("DeliverMessage", message);
    await Clients.Caller.SendAsync("MessageDelivered", message);
  }

}