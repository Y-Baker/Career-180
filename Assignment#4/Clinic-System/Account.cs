namespace ClinicSystem;

public class Account
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string Email { get; set; }
    public string? Number { get; set; }
    public AccountState State { get; set; }
    public Shift Shift { get; set; }
    public DateTime LastLogin { get; set; }
    public string Username { get; set; }
    private string _password;
    public string Password
    {
        get
        {
            return _password;
        }
    }
    public Account(string username, string password, string? name, string email, string? number, Shift shift)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Number = number;
        State = AccountState.Active;
        Shift = shift;
        LastLogin = DateTime.Now;
        Username = username;
        _password = password;
    }

    public bool Login(string password)
    {
        if (password == _password)
        {
            LastLogin = DateTime.Now;
            State = AccountState.Active;
            return true;
        }
        return false;
    }
    public void Logout()
    {
        LastLogin = DateTime.Now;
        State = AccountState.Offline;
    }

    public void RemoveAccount()
    {
        if (!Authorizer.checkAuthorized(this))
        {
            Console.WriteLine("You are not authorized to perform this action");
            return;
        }
        Session? session = Session.GetCurrentSession();
        MemoryStorage.Instance.RemoveSession(MemoryStorage.Instance.CurrentSessionToken);
    }

    public bool ChangePassword(string currentPassword, string newPassword)
    {
        if (!Authorizer.checkAuthorized(this))
        {
            Console.WriteLine("You are not authorized to perform this action");
            return false;
        }
        if (currentPassword != _password)
        {
            Console.WriteLine("Incorrect password");
            return false;
        }
        _password = newPassword;
        return true;
    }
}