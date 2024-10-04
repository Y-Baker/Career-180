namespace ClinicSystem;

internal class ConsoleApp
{
    internal void Run(bool firstTime=false)
    {
        if (firstTime)
            Console.WriteLine("Welcome to Clinic System!");
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Register");
        Console.WriteLine("3. Exit");
        Console.Write("Choose an option: ");
        string option = Console.ReadLine()!;
        switch (option)
        {
            case "1":
                Login();
                break;
            case "2":
                Register();
                break;
            case "3":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid option!");
                break;
        }

        Start();
    }

    internal void Start()
    {
        Session? session = Session.GetCurrentSession();
        if (session is null)
            Login();
        else
        {
            if (Authorizer.checkAuthorized(session))
            {
                if (session.Role == Role.Doctor)
                    DoctorMenu();
                else
                    AssisstantMenu();
            }
            else
                Login(session);
        }
        Start();
    }

    internal void Login(Session? session = null)
    {
        Console.WriteLine("Login Screen");
        if (session is null)
        {
            Session? newSession;
            int attempts = 0;
            do
            {
                StdinService.ReadLogin(out string username, out string password, out bool isRemembered);
                newSession = Session.CreateSession(username, password, isRemembered);
                attempts++;
            } while (newSession is null && attempts < 3);
            if (newSession is null)
            {
                Console.WriteLine("Login failed");
                Run();
            } 
            else
                Console.WriteLine("Login successful");
        }
        else
        {
            if (session.Login())
                Console.WriteLine("Login successful");
            else
            {
                Console.WriteLine("Login failed");
                Run();
            }
              
        }
    }

    internal void Register()
    {
        Console.WriteLine("Register Screen");
        Console.WriteLine("1. Doctor");
        Console.WriteLine("2. Assistant");
        Console.Write("Choose an option: ");
        string option = Console.ReadLine()!;
        switch (option)
        {
            case "1":
                RegisterDoctor();
                break;
            case "2":
                RegisterAssistant();
                break;
            default:
                Console.WriteLine("Invalid option!");
                break;
        }
    }

    internal void RegisterDoctor()
    {
        Doctor doctor = StdinService.ReadDoctor();
        MemoryStorage.Instance.AddDoctor(doctor);
    }

    internal void RegisterAssistant()
    {
        Assistant assistant = StdinService.ReadAssistant();
        MemoryStorage.Instance.AddAssistant(assistant);
    }

    internal void DoctorMenu()
    {
        Console.WriteLine("Main Menu");
        Console.WriteLine("1. View appointments");
        Console.WriteLine("2. Approve appointment");
        Console.WriteLine("3. Remove appointment");
        Console.WriteLine("4. Logout");
        Console.Write("Choose an option: ");
        string option = Console.ReadLine()!;
        Session? session = Session.GetCurrentSession();
        if (session is null)
            Login();
        switch (option)
        {
            case "1":
                ViewAppointments(session!.Account as Doctor);
                break;
            case "2":
                ApproveAppointment();
                break;
            case "3":
                RemoveAppointment();
                break;
            case "4":
                session!.Logout();
                Run();
                break;
            default:
                Console.WriteLine("Invalid option!");
                break;
        }
    }

    internal void AssisstantMenu()
    {
        Console.WriteLine("Main Menu");
        Console.WriteLine("1. View appointments");
        Console.WriteLine("2. Add appointment");
        Console.WriteLine("3. Remove appointment");
        Console.WriteLine("4. Logout");
        Console.Write("Choose an option: ");
        string option = Console.ReadLine()!;
        Session? session = Session.GetCurrentSession();
        if (session is null)
            Login();
        switch (option)
        {
            case "1":
                ViewAppointments();
                break;
            case "2":
                AddAppointment();
                break;
            case "3":
                RemoveAppointment();
                break;
            case "4":
                session!.Logout();
                break;
            default:
                Console.WriteLine("Invalid option!");
                break;
        }
    }


    internal void ReturnToMenu()
    {
        Session? session = Session.GetCurrentSession();
            if (session is null)
                Login();
            if (session!.Role == Role.Doctor)
                DoctorMenu();
            else
                AssisstantMenu();
    }
    internal void ViewAppointments(Doctor? doctor=null)
    {
        ListAppointments(doctor);
        Console.WriteLine("Write Back to go back");
        if (Console.ReadLine() == "Back")
        {
            ReturnToMenu();
        }
    }

    internal void ApproveAppointment()
    {
        Session? session = Session.GetCurrentSession();
        if (session is null)
            Login();
        ListAppointments(session!.Account as Doctor);
        Console.Write("Enter appointment number to approve or `Back` to return: ");
        string input = Console.ReadLine()!;
        if (input == "Back")
        {
            ReturnToMenu();
        }
        else
        {
            int idx = int.Parse(input) - 1;
            Appoiment appoiment = MemoryStorage.Instance.Appoiments[idx];
            appoiment.State = AppoimentState.Approved;
        }
    }

    internal void RemoveAppointment()
    {
        Session? session = Session.GetCurrentSession();
        if (session is null)
            Login();
        ListAppointments(session!.Account as Doctor);
        Console.Write("Enter appointment number to remove or `Back` to return: ");
        string input = Console.ReadLine()!;
        if (input == "Back")
        {
            ReturnToMenu();
        }
        else
        {
            int idx = int.Parse(input) - 1;
            Appoiment appoiment = MemoryStorage.Instance.Appoiments[idx];
            appoiment.State = AppoimentState.Cancelled;
        }
    }

    internal void AddAppointment()
    {
        Session? session = Session.GetCurrentSession();
        if (session is null)
            Login();
        Department department = ChooseDepartment();
        Doctor doctor = ChooseDoctor(department);
        
        Patient? patient = ChoosePatient();
        if (patient is null)
        {
            Console.WriteLine("** Create a new patient **");
            patient = StdinService.ReadPatient();
            MemoryStorage.Instance.AddPatient(patient);
        }

        DateTime time;
        int attempts = 0;
        do
        {
            Console.WriteLine("Enter appointment time: ");
            time = StdinService.ReadTime();
        } while (!ValidTime(time, doctor) && attempts < 3);
        if (attempts == 3)
        {
            Console.WriteLine("Failed to add appointment");
            ReturnToMenu();
        }
        Assistant assistant = session!.Account as Assistant ?? throw new Exception("Session account is not an Assistant.");
        Appoiment appoiment = new Appoiment(time, patient, assistant, doctor, 100);
        MemoryStorage.Instance.AddAppoiment(appoiment);
    }

    internal Patient? ChoosePatient()
    {
        Console.WriteLine("Enter patient Number: ");
        string patientNumber = Console.ReadLine()!;
        Patient? patient = MemoryStorage.Instance.GetPatientByNumber(patientNumber);
        return patient;
    }
    internal Department ChooseDepartment()
    {
        ListDepartments();
        Console.Write("Choose a department: ");
        int departmentIdx = int.Parse(Console.ReadLine()!) - 1;
        Department department = (Department)departmentIdx;
        return department;
    }
    internal Doctor ChooseDoctor(Department? department=null)
    {
        ListDoctors(department);
        List<Doctor> doctors = MemoryStorage.Instance.Doctors;
        Console.Write("Choose a doctor: ");
        int doctorIdx = int.Parse(Console.ReadLine()!) - 1;
        Doctor doctor = doctors[doctorIdx];
        return doctor;
    }


    internal void ListPatients()
    {
        List<Patient> patients = MemoryStorage.Instance.Patients;
        for (int i = 0; i < patients.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {patients[i].Name}");
        }
    }

    internal void ListDepartments()
    {
        foreach (var depart in Enum.GetValues(typeof(Department)))
        {
            Console.WriteLine($"{(int)depart} - {depart}");
        }

    }

    internal void ListDoctors(Department? department=null)
    {
        if (department == null)
        {
            List<Doctor> doctors = MemoryStorage.Instance.Doctors;
            for (int i = 0; i < doctors.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {doctors[i].Name}");
            }
        }
    }
    internal void ListAppointments(Doctor? doctor=null)
    {
        if (doctor is null)
        {
            List<Appoiment> allAppoiments = MemoryStorage.Instance.Appoiments;
            if (allAppoiments.Count == 0)
                Console.WriteLine("No Appoiments Found");
            for (int i = 0; i < allAppoiments.Count; i++)
            {
                Console.WriteLine($"{i+1}. {ViewAppointment(allAppoiments[i])}");
            }
        }
        else
        {
            List<Appoiment> appoiments = MemoryStorage.Instance.GetAppoimentsByDoctor(doctor);
            if (appoiments.Count == 0)
                Console.WriteLine("No Appoiments Found");
            for (int i = 0; i < appoiments.Count; i++)
            {
                Console.WriteLine($"{i+1}. {ViewAppointment(appoiments[i])}");
            }
        }
    }

    internal string ViewAppointment(Appoiment appoiment)
    {
        return $"{appoiment.Time} - {appoiment.Patient.Name} - {appoiment.State} - {appoiment.AppoimentIsPaid} - {appoiment.Price}";
    }

    internal bool ValidTime(DateTime time, Doctor doctor)
    {
        if (time < DateTime.Now)
        {
            Console.WriteLine("Time Cannot be in the past");
            return false;
        }
        if (time > DateTime.Now.AddMonths(1))
        {
            Console.WriteLine("Time Cannot be more than a month from now");
            return false;
        }

        if (!doctor.TimeIsAvailable(time))
        {
            Console.WriteLine("Doctor is not available at this time");
            return false;
        }

        if (!Appoiment.TimeIsAvailable(time, doctor.Department))
        {
            Console.WriteLine("Doctor is not available at this time");
            return false;
        }
        return true;
    }
}
