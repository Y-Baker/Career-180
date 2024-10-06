namespace ClinicSystem;

public class Doctor : Account
{
    public Department Department { get; set; }
    public Auth Auth { get; set; }
    public HashSet<DayOfWeek> WorkingDays { get; set; }

    public Doctor(string username, string password, string name, string email, string number, Department department, HashSet<DayOfWeek> workingDays, Auth auth=Auth.Partial, Shift shift=Shift.Morning)
        : base(username, password, name, email, number, shift)
    {
        Department = department;
        Auth = auth;
        WorkingDays = workingDays;
    }

    public Doctor(Account account, Department department, HashSet<DayOfWeek> workingDays, Auth auth=Auth.Partial)
        : base(account.Username, account.Password, account.Name ?? "", account.Email, account.Number ?? "", account.Shift)
    {
        Department = department;
        Auth = auth;
        WorkingDays = workingDays;
    }

    public void AddWorkingDays(DayOfWeek day)
    {
        WorkingDays.Add(day);
        // todo: save to storage
    }

    public void RemoveWorkingDays(DayOfWeek day)
    {
        WorkingDays.Remove(day);
        // todo: save to storage
    }

    public void UpdateWorkingDays(DayOfWeek oldDay, DayOfWeek newDay)
    {
        WorkingDays.Remove(oldDay);
        WorkingDays.Add(newDay);
        // todo: save to storage
    }

    public bool GrantAccess(Doctor doctor)
    {
        if (this.Auth == Auth.Partial)
        {
            Console.WriteLine("You are not authorized to perform this action");
            return false;
        }
        doctor.Auth = Auth.Full;
        // todo: save to storage
        return true;
    }

    public void GetSchedules()
    {
        if (!Authorizer.checkAuthorized(this))
            return;
        Console.WriteLine("Schedules");
    }

    public bool TimeIsAvailable(DateTime time)
    {
        DayOfWeek day = time.DayOfWeek;
        if (!WorkingDays.Contains(day))
            return false;
        switch (Shift)
        {
            case Shift.Morning:
                if (time.Hour < 9 || time.Hour >= 14)
                    return false;
                break;
            case Shift.Evening:
                if (time.Hour < 14 || time.Hour >= 19)
                    return false;
                break;
            case Shift.Night:
                if (time.Hour < 19 || time.Hour >= 24)
                    return false;
                break;
        }
        return true;
    }
}
