using Microsoft.AspNetCore.SignalR.Client;

namespace CryptoP2P.Backend.Services;

public interface IConnectionManager
{
  Task ConnectPeer(string url);
  Task InvokeAsync(string endpoint, object? arg);
  Task InvokeAsync(string endpoint);
}