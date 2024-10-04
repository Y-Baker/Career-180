using System.ComponentModel;

namespace ClinicSystem;

internal class Session
{
    public Account Account { get; set; }
    public Role Role { get; set; }
    public DateTime LoginTime { get; set; }
    public DateTime LastActionTime { get; set; }
    public Guid Token { get; set; }
    public bool IsLoggedIn { get; set; }
    public bool IsRemembered { get; set; }


    public Session(Account account, bool isRemembered, Role role)
    {
        Account = account;
        LoginTime = DateTime.Now;
        LastActionTime = DateTime.Now;
        Token = Guid.NewGuid();
        IsLoggedIn = true;
        IsRemembered = isRemembered;
        Role = role;
    }

    public static Session? CreateSession(string username, string password, bool isRemembered = false)
    {
        Account? account = MemoryStorage.Instance.GetAccountByUsername(username);
        if (account is null)
        {
            Console.WriteLine("Account not found");
            return null;
        }
        if (account.Password != password)
        {
            Console.WriteLine("Incorrect password");
            return null;
        }
        Role role = MemoryStorage.Instance.GetRoleByUsername(username) ?? throw new Exception("Can't Find Role of Account");
        Session newSession = new Session(account, isRemembered, role);
        MemoryStorage.Instance.AddSession(newSession);
        MemoryStorage.Instance.SetCurrentSession(newSession);
        return newSession;
    }

    public static Session? GetCurrentSession()
    {
        return MemoryStorage.Instance.GetSessionByToken(MemoryStorage.Instance.CurrentSessionToken);
    }

    public bool Login()
    {
        if (this.IsRemembered && DateTime.Now - this.LoginTime < TimeSpan.FromDays(30))
        {
            IsLoggedIn = true;
            MemoryStorage.Instance.SetCurrentSession(this);
            return true;
        }
        else
        {
            string password;
            int attempts = 0;
            do
            {
                Console.Write("Enter your password: ");
                password = StdinService.ReadPassword();
                attempts++;
            } while (!Account.Login(password) && attempts <= 3);
            if (Account.Login(password))
            {
                IsLoggedIn = true;
                LoginTime = DateTime.Now;
                MemoryStorage.Instance.SetCurrentSession(this);
                return true;
            }
            Console.WriteLine("Too many attempts");
            return false;
        }
    }

    public bool Logout(bool force=false)
    {
        if (force || Authorizer.checkAuthorized(this))
        {
            IsLoggedIn = false;
            Account.Logout();
            return true;
        }
        return false;
    }

    public void UpdateLastActionTime()
    {
        LastActionTime = DateTime.Now;
    }

    public void SwitchSession()
    {
        IsLoggedIn = false;
    }

    public void CheckSession()
    {
        if (DateTime.Now - LastActionTime > TimeSpan.FromHours(8))
        {
            Logout(true);
        }
    }
}
