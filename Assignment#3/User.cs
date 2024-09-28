namespace Assignment_3;
public class User
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    private string? _password;
    public required string? Password
    {
        get
        {
            if (!SessionManger.checkAuthorized(this))
            {
                Console.WriteLine("You are not authorized to view this information");
                return null;
            }
            return _password;
        }
        set
        {
            int attempts = 3;
            string currentPassword;
            if (!SessionManger.checkAuthorized(this))
            {
                Console.WriteLine("You are not authorized to change this information");
                return;
            }
            do
            {
                Console.Write("Enter your current password: ");
                currentPassword = StdinService.ReadPassword();
                if (currentPassword != _password)
                {
                    Console.WriteLine("Incorrect password");
                    attempts--;
                }
            } while (currentPassword != _password && attempts >= 0);
            if (attempts < 0)
            {
                Console.WriteLine("Too many attempts");
                return;
            }
            _password = value;
        }
    }
    protected double _wallet;
    public double Wallet
    {
        get => _wallet;
    }
    public required string Country { get; set; }
    public DateTime Birthdate { get; set; }
    public string? MainLanguage { get; set; }
    public UserState State { get; set; }
    public DateTime lastSeen { get; set; }
    public List<User> Friends { get; set; }
    public List<User> BlockedUsers { get; set; }
    public List<User> FriendRequests { get; set; }

    public User(string name, string email, string password, string country, DateTime birthdate, string? mainLanguage)
    {
        Name = name;
        Email = email;
        _password = password;
        Country = country;
        Birthdate = birthdate;
        MainLanguage = mainLanguage;
        State = UserState.Offline;
        lastSeen = DateTime.Now;
        Friends = new List<User>();
        BlockedUsers = new List<User>();
        FriendRequests = new List<User>();
    }
    public User(string name, string email, string password, DateTime birthdate)
    {
        Name = name;
        Email = email;
        _password = password;
        Birthdate = birthdate;
        MainLanguage = "English";
        State = UserState.Offline;
        lastSeen = DateTime.Now;
        Friends = new List<User>();
        BlockedUsers = new List<User>();
        FriendRequests = new List<User>();
    }

    public void changePassword()
    {
        int attempts = 3;
        string currentPassword;
        string newPassword;
        string confirmPassword;
        if (!SessionManger.checkAuthorized(this))
        {
            Console.WriteLine("You are not authorized to change this information");
            return;
        }
        do
        {
            Console.Write("Enter your current password: ");
            currentPassword = StdinService.ReadPassword();
            if (currentPassword != _password)
            {
                Console.WriteLine("Incorrect password");
                attempts--;
            }
        } while (currentPassword != _password && attempts >= 0);
        if (attempts < 0)
        {
            Console.WriteLine("Too many attempts");
            return;
        }
        do
        {
            Console.Write("Enter your new password: ");
            newPassword = StdinService.ReadPassword();
            Console.Write("Confirm your new password: ");
            confirmPassword = StdinService.ReadPassword();
        } while (newPassword != confirmPassword);
        _password = newPassword;
    }

    public int getAge()
    {
        return DateTime.Now.Year - Birthdate.Year - (DateTime.Now.DayOfYear < Birthdate.DayOfYear ? 1 : 0);
    }

    public void AddFriend(User user)
    {
        user.FriendRequests.Add(this);
    }

    public void AcceptFriendRequest(User user)
    {
        if (!FriendRequests.Contains(user))
        {
            Console.WriteLine("No friend request found");
            return;
        }
        Friends.Add(user);
        user.Friends.Add(this);
        FriendRequests.Remove(user);
    }

    public void BlockUser(User user)
    {
        if (Friends.Contains(user))
        {
            Friends.Remove(user);
            user.Friends.Remove(this);
        }
        BlockedUsers.Add(user);
    }

    public void UnblockUser(User user)
    {
        BlockedUsers.Remove(user);
    }
}
