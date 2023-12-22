using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

public class SocketHub : Hub
{
    private readonly IHubContext<SocketHub> _hubContext;
    private readonly IUserManager _userManager;

    public SocketHub(IHubContext<SocketHub> hubContext, IUserManager userManager)
    {
        _hubContext = hubContext;
        _userManager = userManager;
    }

    // Listening to clients, this will run when clients send a "ClientMessage"
    public async Task ClientMessage(string message)
    {
        // You can retrieve the user associated with the current connection using Context.ConnectionId
        var user = _userManager.GetUserByConnectionId(Context.ConnectionId);

        Console.WriteLine($"Received 'ClientMessage' from client {user?.Username}: {message}");
    }

    public override async Task OnConnectedAsync()
    {
        var newUser = new User("", Context.ConnectionId, 0, 0, 0);

        _userManager.AddUser(newUser);

        // Send the updated user list to all clients
        await _hubContext.Clients.All.SendAsync("UserListUpdated", _userManager.GetUsers());

        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        // Remove the disconnected user from the list
        _userManager.RemoveUser(Context.ConnectionId);

        // Send the updated user list to all clients
        await _hubContext.Clients.All.SendAsync("UserListUpdated", _userManager.GetUsers());

        await base.OnDisconnectedAsync(exception);
    }
}
