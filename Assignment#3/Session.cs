namespace Assignment_3;

public class Session
{
    public User User { get; set; }
    public DateTime LoginTime { get; set; }
    public DateTime LastActionTime { get; set; }
    public string? Token { get; set; }
    public bool IsLoggedIn { get; set; }
    public bool IsRemembered { get; set; }


    public Session(User user, string token, bool isRemembered)
    {
        User = user;
        LoginTime = DateTime.Now;
        LastActionTime = DateTime.Now;
        Token = token;
        IsLoggedIn = true;
        IsRemembered = isRemembered;
    }

    public bool Login()
    {
        if (this.IsRemembered)
            return true;
        else
        {
            string password;
            int attempts = 0;
            do
            {
                Console.Write("Enter your password: ");
                password = StdinService.ReadPassword();
                attempts++;
            } while (password != User.Password && attempts <= 3);
            if (password == User.Password)
                return true;
            return false;
        }
    }

    public void UpdateLastActionTime()
    {
        LastActionTime = DateTime.Now;
    }

    public void Logout()
    {
        IsLoggedIn = false;
        IsRemembered = false;
    }

    public void SwitchSession()
    {
        IsLoggedIn = false;
    }

    public void CheckSession()
    {
        if (DateTime.Now - LastActionTime > TimeSpan.FromMinutes(30))
        {
            Logout();
        }
    }

    ~Session()
    {
        Console.WriteLine("Session is destroyed");
    }
}
