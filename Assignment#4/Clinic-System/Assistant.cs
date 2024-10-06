namespace ClinicSystem;

public class Assistant : Account
{
    public Auth Auth { get; set; }
    public List<Patient> waitingList { get; set; }

    public Assistant(string username, string password, string name, string email, string number, Shift shift=Shift.Morning)
        : base(username, password, name, email, number, shift)
    {
        Auth = Auth.Partial;
        waitingList = new();
    }
    public Assistant(Account account)
        : base(account.Username, account.Password, account.Name ?? "", account.Email, account.Number ?? "", account.Shift)
    {
        Auth = Auth.Partial;
        waitingList = new();
    }

    public bool PatientExists(string number)
    {
        Patient? patient = MemoryStorage.Instance.GetPatientByNumber(number);
        return (patient is not null) ? true : false;
    }
    public void AddPatient(Patient patient)
    {
        MemoryStorage.Instance.AddPatient(patient);
    }
    public void AddPatientToWaitingList(Patient patient)
    {
        waitingList.Add(patient);
    }

    public void addAppoiment(Appoiment appoiment)
    {
        MemoryStorage.Instance.AddAppoiment(appoiment);
    }

}
