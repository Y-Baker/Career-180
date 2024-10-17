namespace ClinicSystem;

public class Doctor : Account
{
    public Department Department { get; set; }
    public Auth Auth { get; set; }
    public HashSet<DayOfWeek> WorkingDays { get; set; }

    public Doctor(string username, string password, string name, string email, string number, Department department, HashSet<DayOfWeek>? workingDays=null, Auth auth=Auth.Partial, Shift shift=Shift.Morning)
        : base(username, password, name, email, number, shift)
    {
        Department = department;
        Auth = auth;
        WorkingDays = workingDays ?? new HashSet<DayOfWeek>();
    }

    public Doctor(Account account, Department department, HashSet<DayOfWeek> workingDays, Auth auth=Auth.Partial)
        : base(account.Username, account.Password, account.Name ?? "", account.Email, account.Number ?? "", account.Shift)
    {
        Department = department;
        Auth = auth;
        WorkingDays = workingDays;
    }

    public string DisplayInfo()
    {
        List<string> days_3_letter = new List<string>();
        foreach (DayOfWeek day in WorkingDays)
            days_3_letter.Add(day.ToString().Substring(0, 3));

        string workingDays = string.Join(", ", days_3_letter);
        return $"{Name} - {Shift} - {workingDays}";
    }
    public void AddWorkingDays(DayOfWeek day)
    {
        WorkingDays.Add(day);
    }

    public void RemoveWorkingDays(DayOfWeek day)
    {
        WorkingDays.Remove(day);
    }

    public void UpdateWorkingDays(DayOfWeek oldDay, DayOfWeek newDay)
    {
        WorkingDays.Remove(oldDay);
        WorkingDays.Add(newDay);
    }

    public bool GrantAccess(Doctor doctor)
    {
        if (this.Auth == Auth.Partial)
        {
            Console.WriteLine("You are not authorized to perform this action");
            return false;
        }
        doctor.Auth = Auth.Full;
        return true;
    }

    public List<Appoiment> GetSchedules(DateOnly day)
    {
        if (!Authorizer.checkAuthorized(this))
            return new();
        List<Appoiment> appoiments = MemoryStorage.Instance.GetAppoiments(this, day);
        return appoiments.Where(appoiment => appoiment.State != AppoimentState.Canceled).ToList();
    }

    public bool TimeIsAvailable(DateTime time)
    {
        TimeSpan start, end;
        DayOfWeek day = time.DayOfWeek;
        if (!WorkingDays.Contains(day))
            return false;
        switch (Shift)
        {
            case Shift.Morning:
                start = MemoryStorage.Instance.WorkingHours[Shift.Morning].Item1;
                end = MemoryStorage.Instance.WorkingHours[Shift.Morning].Item2;
                if (end < start)
                {
                    if (time.TimeOfDay > end && time.TimeOfDay < start)
                        return false;
                }
                else
                {
                    if (time.TimeOfDay < start || time.TimeOfDay > end)
                        return false;
                }
                break;
            case Shift.Evening:
                start = MemoryStorage.Instance.WorkingHours[Shift.Evening].Item1;
                end = MemoryStorage.Instance.WorkingHours[Shift.Evening].Item2;
                if (end < start)
                {
                    if (time.TimeOfDay > end && time.TimeOfDay < start)
                        return false;
                }
                else
                {
                    if (time.TimeOfDay < start || time.TimeOfDay > end)
                        return false;
                }
                break;
            case Shift.Night:
                start = MemoryStorage.Instance.WorkingHours[Shift.Night].Item1;
                end = MemoryStorage.Instance.WorkingHours[Shift.Night].Item2;
                if (end < start)
                {
                    if (time.TimeOfDay > end && time.TimeOfDay < start)
                        return false;
                }
                else
                {
                    if (time.TimeOfDay < start || time.TimeOfDay > end)
                        return false;
                }
                break;
        }
        return true;
    }
}