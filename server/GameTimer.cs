using Microsoft.AspNetCore.SignalR;

public class GameTimer
{
    private readonly IHubContext<SocketHub> _hubContext;
    private Timer _timer;
    private readonly IUserManager _userManager;

    public GameTimer(IHubContext<SocketHub> hubContext, IUserManager userManager)
    {
        _hubContext = hubContext;
        _userManager = userManager;
    }

    public void StartTimer()
    {
        _timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }

    private async void TimerCallback(object state)
    {
        var users = _userManager.GetUsers();
        await _hubContext.Clients.All.SendAsync("Users", users);

        Console.WriteLine("Sending Users " + DateTime.Now.ToLongTimeString());
        foreach (var user in users)
        {
            Console.WriteLine(
                $"User: {user.Username}, Score: {user.Score}, Position: ({user.X}, {user.Y})"
            );
        }
    }
}
