using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRChat.Services;

public interface IConnectionManager
{
  Task ConnectPeer(string url);
  Task InvokeAsync(string endpoint, object? arg);
  Task InvokeAsync(string endpoint);

  void RegisterHandlers();
}