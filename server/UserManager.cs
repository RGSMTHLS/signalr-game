public interface IUserManager
{
    void AddUser(User user);
    void RemoveUser(string username);
    void UpdateUser(string username, int newScore, int newX, int newY);
    IReadOnlyList<User> GetUsers();
    User GetUserByConnectionId(string connectionId);
}

public class UserManager : IUserManager
{
    private readonly List<User> _users = new List<User>();

    public UserManager()
    {
        // Initialize with some test users
        AddUser(new User("user1", "", 100, 10, 20));
        AddUser(new User("user2", "", 150, 15, 25));
        AddUser(new User("user3", "", 80, 8, 15));
    }

    public void AddUser(User user)
    {
        var newUser = new User(
            $"User_{Guid.NewGuid().ToString().Substring(0, 8)}",
            user.ConnectionId, // Use connection ID as a unique identifier
            user.Score,
            user.X,
            user.Y
        );

        _users.Add(newUser);
    }

    public void RemoveUser(string connectionId)
    {
        var userToRemove = _users.FirstOrDefault(u => u.ConnectionId == connectionId);
        if (userToRemove != null)
        {
            _users.Remove(userToRemove);
        }
    }

    public void UpdateUser(string connectionId, int newScore, int newX, int newY)
    {
        var userToUpdate = _users.FirstOrDefault(u => u.ConnectionId == connectionId);
        if (userToUpdate != null)
        {
            userToUpdate.Score = newScore;
            userToUpdate.X = newX;
            userToUpdate.Y = newY;
        }
    }

    public IReadOnlyList<User> GetUsers()
    {
        return _users.AsReadOnly();
    }

    public User GetUserByConnectionId(string connectionId)
    {
        return _users.FirstOrDefault(u => u.ConnectionId == connectionId);
    }
}

public class User
{
    public User(string Username, string ConnectionId, int Score, int X, int Y)
    {
        this.Username = Username;
        this.ConnectionId = ConnectionId;
        this.Score = Score;
        this.X = X;
        this.Y = Y;
    }
    public string Username { get; set; }
    public string ConnectionId { get; set; } // Unique identifier for SignalR connection
    public int Score { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}
