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
            StdinService.Decorate("Account not found", ConsoleColor.Red, end: "\n\n");
            return null;
        }
        if (account.Password != password)
        {
            StdinService.Decorate("Incorrect password", ConsoleColor.Red, end: "\n\n");
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

    public bool Login(string password)
    {
        if (Account.Login(password))
        {
            IsLoggedIn = true;
            UpdateLastLogin();
            MemoryStorage.Instance.SetCurrentSession(this);
            return true;
        }
        return false;
    }
    public bool Login(out Interrupt interrupt)
    {
        interrupt = Interrupt.Success;
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
                interrupt = StdinService.ReadPassword(out password);
                if (interrupt == Interrupt.Back)
                    return false;
                if (interrupt == Interrupt.Empty)
                    continue;
                attempts++;
            } while (!Account.Login(password) && attempts < 3);
    
            if (Account.Login(password))
            {
                IsLoggedIn = true;
                UpdateLastLogin();
                MemoryStorage.Instance.SetCurrentSession(this);
                return true;
            }
            StdinService.Decorate("Too many attempts", ConsoleColor.Red, end: "\n\n");
            Logout(true, true);
            interrupt = Interrupt.Exit;
            return false;
        }
    }

    public bool Logout(bool force=false, bool full=false)
    {
        UpdateLastActionTime();
        if (force || Authorizer.checkAuthorized(this))
        {
            IsLoggedIn = false;
            if (full)
                IsRemembered = false;
            Account.Logout();
            return true;
        }
        return false;
    }

    public void UpdateLastLogin()
    {
        UpdateLastActionTime();
        LoginTime = DateTime.Now;
    }
    public void UpdateLastActionTime()
    {
        LastActionTime = DateTime.Now;
    }

    public void CheckSession()
    {
        UpdateLastActionTime();
        if (DateTime.Now - LastActionTime > TimeSpan.FromHours(8))
        {
            Logout(true);
        }
    }

    public static string HeadView()
    {
        return $"{"Username",-20} {"Name",-20} {"Role",-20} {"Last Login",-30} {"Last Action",-30} {"Is Remembered",-20}";
    }
    public string View()
    {
        return $"{Account.Username,-20} {Account.Name,-20} {Role,-20} {LoginTime,-30} {LastActionTime,-30} {IsRemembered,-20}";
    }
}
