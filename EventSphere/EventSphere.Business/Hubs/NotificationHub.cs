using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace EventSphere.API.Hubs;

public class NotificationHub : Hub
{
    public static readonly ConcurrentDictionary<string, string> _connections = new();

    public override Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst("ID")?.Value;
        if (userId != null)
        {
            _connections[Context.ConnectionId] = userId;
        }
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        _connections.TryRemove(Context.ConnectionId, out _);
        return base.OnDisconnectedAsync(exception);
    }

    public static string GetUserIdByConnectionId(string connectionId)
    {
        _connections.TryGetValue(connectionId, out var userId);
        return userId;
    }
}
    
