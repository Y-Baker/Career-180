namespace ClinicSystem.engine;
internal class MemoryStorage
{
    private static readonly Lazy<MemoryStorage> _instance = new Lazy<MemoryStorage>(() => new MemoryStorage());
    public static MemoryStorage Instance { get { return _instance.Value; } }
    public Guid CurrentSessionToken { get; set; }
    public List<Session> Sessions { get; set; }
    public List<Doctor> Doctors { get; set; }
    public List<Assistant> Assistants { get; set; }
    public List<Patient> Patients { get; set; }
    public List<Appoiment> Appoiments { get; set; }

    private MemoryStorage()
    {
        Sessions = new List<Session>();
        Doctors = new List<Doctor>();
        Assistants = new List<Assistant>();
        Patients = new List<Patient>();
        Appoiments = new List<Appoiment>();
    }
    public void AddDoctor(Doctor doctor)
    {
        Doctors.Add(doctor);
    }
    public void AddAssistant(Assistant assistant)
    {
        Assistants.Add(assistant);
    }
    public void AddSession(Session session)
    {
        Sessions.Add(session);
    }
    public void SetCurrentSession(Session session)
    {
        CurrentSessionToken = session.Token;
    }
    public Account? GetAccountByUsername(string username)
    {
        Account? account = Doctors.Cast<Account>().Concat(Assistants.Cast<Account>()).FirstOrDefault(acc => acc.Username.Equals(username));
        if (account != null)
        {
            return account;
        }
        return null;
    }
    public Role? GetRoleByUsername(string username)
    {
        Doctor? doctor = Doctors.Find(e => e.Username.Equals(username));
        if (doctor is not null)
            return Role.Doctor;
        Assistant? assisstant = Assistants.Find(e => e.Username.Equals(username));
        if (assisstant is not null)
            return Role.Assistant;
        return null;
    }
    //public Doctor? GetDoctorByUsername(string username)
    //{
    //    return Doctors.Find(doctor => doctor.Username.Equals(username)) ?? null;
    //}
    //public Assistant? GetAssistantByUsername(string username)
    //{
    //    return Assistants.Find(assistant => assistant.Username.Equals(username)) ?? null;
    //}

    public Account? GetAccountByToken(Guid token)
    {
        return Sessions.Find(session => session.Token.Equals(token))?.Account ?? null;
    }
    public Session? GetSessionByToken(Guid token)
    {
        return Sessions.Find(session => session.Token.Equals(token)) ?? null;
    }
    public void RemoveSession(Guid token)
    {
        Session? session = Sessions.Find(session => session.Token.Equals(token));
        if (session is not null)
            Sessions.Remove(session);
    }

    public void AddPatient(Patient patient)
    {
        Patients.Add(patient);
    }

    public Patient? GetPatientByNumber(string number)
    {
        return Patients.Find(patient => patient.Number.Equals(number)) ?? null;
    }


    public void AddAppoiment(Appoiment appoiment)
    {
        Appoiments.Add(appoiment);
    }

    public List<Appoiment> GetAppoimentsByDoctor(Doctor doctor)
    {
        return Appoiments.FindAll(appoiment => appoiment.Doctor.Equals(doctor));
    }

    public List<Appoiment> GetAppoimentsByPatient(Patient patient)
    {
        return Appoiments.FindAll(appoiment => appoiment.Patient.Equals(patient));
    }

    public List<Appoiment> GetAppoimentsByAssistant(Assistant assistant)
    {
        return Appoiments.FindAll(appoiment => appoiment.Assistant.Equals(assistant));
    }

    public List<Appoiment> GetAppoimentsByDepartment(Department department)
    {
        return Appoiments.FindAll(appoiment => appoiment.Doctor.Department.Equals(department));
    }
}