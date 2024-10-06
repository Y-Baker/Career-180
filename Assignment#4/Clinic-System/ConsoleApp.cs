namespace ClinicSystem;

internal class ConsoleApp
{
    private Stack<Action> actions = new Stack<Action>();

    internal void Run()
    {
        Console.Clear();
        StdinService.Decorate("Welcome to Clinic System!");

        Console.WriteLine("1. Login");
        Console.WriteLine("2. Register");
        Console.WriteLine("3. Switch User");
        Console.WriteLine("4. Exit");
        Console.WriteLine();
        Console.Write("Choose an option: ");

        Interrupt interrupt = StdinService.ReadInputWithShortcut(out string option);
        HandleInterrupt(interrupt);        

        switch (option)
        {
            case "1":
                PushToStack(Login);
                Login();
                break;
            case "2":
                PushToStack(Register);
                Register();
                break;
            case "3":
                // PushToStack(SwitchUser);
                // SwitchSession();
                break;
            case "4":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid option!");
                Thread.Sleep(1000);
                Run();
                break;
        }

        // Start();
    }


    internal void Login()
    {
        Console.Clear();
        StdinService.Decorate("Login Screen");
        Session? session = Session.GetCurrentSession();
        if (session is null)
        {
            Session? newSession;
            int attempts = 0;
            do
            {
                ReadLogin(out string username, out string password, out bool isRemembered);
                newSession = Session.CreateSession(username, password, isRemembered);
                attempts++;
            } while (newSession is null && attempts < 3);
            if (newSession is null)
            {
                Console.WriteLine("Login failed");
                Thread.Sleep(1000);
                PushToStack(Run);
                Run();
            } 
            else
                Console.WriteLine("Login successful");
        }
        else // todo: add username in the top right corner
        {
            if (session.Login())
                Console.WriteLine("Login successful");
            else
            {
                Console.WriteLine("Login failed");
                Thread.Sleep(1000);
                PushToStack(Run);
                Run();
            }
              
        }

        PushToStack(MainMenu);
        MainMenu();
    }

    internal void Register()
    {
        Console.Clear();
        StdinService.Decorate("Register Screen");

        Console.WriteLine("1. Doctor");
        Console.WriteLine("2. Assistant");
        Console.WriteLine();
        Console.Write("Choose an option: ");

        Interrupt interrupt = StdinService.ReadInputWithShortcut(out string option);
        HandleInterrupt(interrupt); 

        switch (option)
        {
            case "1":
                PushToStack(RegisterDoctor);
                RegisterDoctor();
                break;
            case "2":
                PushToStack(RegisterAssistant);
                RegisterAssistant();
                break;
            default:
                Console.WriteLine("Invalid option!");
                Thread.Sleep(1000);
                Register();
                break;
        }

        PushToStack(Login);
        Login();
    }

    internal void MainMenu()
    {
        Session? session = Session.GetCurrentSession();
        if (session is null)
            Login();
        else
        {
            if (Authorizer.checkAuthorized(session))
            {
                if (session.Role == Role.Doctor)
                {
                    PushToStack(DoctorMenu);
                    DoctorMenu();
                }
                else
                {
                    PushToStack(AssisstantMenu);
                    AssisstantMenu();
                }
            }
            else
                Login();
        }
        MainMenu();
    }

    internal void RegisterDoctor()
    {
        Console.Clear();
        StdinService.Decorate("Register Doctor Screen");

        Doctor doctor = ReadDoctor();
        MemoryStorage.Instance.AddDoctor(doctor);
    }

    internal void RegisterAssistant()
    {
        Console.Clear();
        StdinService.Decorate("Register Doctor Screen");

        Assistant assistant = ReadAssistant();
        MemoryStorage.Instance.AddAssistant(assistant);
    }

    internal void DoctorMenu()
    {
        string option;
        Interrupt interrupt;

        Console.Clear();
        StdinService.Decorate("Doctor Menu");

        Console.WriteLine("1. View appointments");
        Console.WriteLine("2. Approve appointment");
        Console.WriteLine("3. Remove appointment");
        Console.WriteLine("4. View Profile");
        Console.WriteLine("5. Logout");
        Console.Write("Choose an option: ");

        do
        {
            interrupt = StdinService.ReadInputWithShortcut(out option);
        } while (interrupt == Interrupt.Empty);
        HandleInterrupt(interrupt);

        Session? session = Session.GetCurrentSession();
        if (session is null)
            Login();

        switch (option)
        {
            case "1":
                PushToStack(ViewAppointments);
                ViewAppointments();
                break;
            case "2":
                PushToStack(ApproveAppointment);
                ApproveAppointment();
                break;
            case "3":
                PushToStack(RemoveAppointment);
                RemoveAppointment();
                break;
            case "4":
                // PushToStack(ViewProfile);
                // ViewProfile();
                break;
            case "5":
                session?.Logout();
                PushToStack(Run);
                Run();
                break;
            default:
                Console.WriteLine("Invalid option!");
                DoctorMenu();
                break;
        }
    }

    internal void AssisstantMenu()
    {
        string option;
        Interrupt interrupt;

        Console.Clear();
        StdinService.Decorate("Assistant Menu");

        Console.WriteLine("1. View appointments");
        Console.WriteLine("2. Add appointment");
        Console.WriteLine("3. Remove appointment");
        Console.WriteLine("4. View Profile");
        Console.WriteLine("5. Logout");
        Console.Write("Choose an option: ");

        do
        {
            interrupt = StdinService.ReadInputWithShortcut(out option);
        } while (interrupt == Interrupt.Empty);
        HandleInterrupt(interrupt);

        Session? session = Session.GetCurrentSession();
        if (session is null)
            Login();

        switch (option)
        {
            case "1":
                PushToStack(ViewAppointments);
                ViewAppointments();
                break;
            case "2":
                PushToStack(AddAppointment);
                AddAppointment();
                break;
            case "3":
                PushToStack(RemoveAppointment);
                RemoveAppointment();
                break;
            case "4":
                // PushToStack(ViewProfile);
                // ViewProfile();
                break;
            case "5":
                session?.Logout();
                PushToStack(Run);
                Run();
                break;
            default:
                Console.WriteLine("Invalid option!");
                AssisstantMenu();
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

    internal void ViewAppointments()
    {
        Session? session = Session.GetCurrentSession();
        if (session is null)
            Login();
        if (session!.Role == Role.Doctor)
        {
            Doctor doctor = session.Account as Doctor ?? throw new Exception("Session account is not a Doctor.");
            ListAppointments(doctor);
        }
        else
            ListAppointments();

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
    
    internal void HandleInterrupt(Interrupt interrupt)
    {
        switch (interrupt)
        {
            case Interrupt.Success:
                break;
            case Interrupt.Empty:
                if (actions.Count > 0)
                    PopFromStack();
                else
                    Run();
                break;
            case Interrupt.Back:
                if (actions.Count > 0)
                    PopFromStack();
                else
                    Run();
                break;
            case Interrupt.Exit:
                Environment.Exit(0);
                break;
        }
    }

    internal void ReadLogin(out string username, out string password, out bool isRemembered)
    {
        Interrupt interrupt;
        do
        {
            Console.Write("Enter your username: ");
            interrupt = StdinService.ReadInputWithShortcut(out username);
        } while (interrupt == Interrupt.Empty);
        HandleInterrupt(interrupt);

        do
        {
            Console.Write("Enter your password: ");
            interrupt = StdinService.ReadPassword(out password);
        } while (interrupt == Interrupt.Empty);
        HandleInterrupt(interrupt);

        Console.Write("Remember me? (y/n): ");
        isRemembered = Console.ReadLine()!.ToLower() == "y";
    }

    internal Account ReadAccount()
    {
        string username, password, name, email, number, option;
        Interrupt interrupt;
        do
        {
            Console.Write("Enter your username: ");
            interrupt = StdinService.ReadInputWithShortcut(out username);
        } while (interrupt == Interrupt.Empty);
        HandleInterrupt(interrupt);

        do
        {
            Console.Write("Enter your password: ");
            interrupt = StdinService.ReadPassword(out password);
        } while (interrupt == Interrupt.Empty);
        HandleInterrupt(interrupt);

        do
        {
            Console.Write("Enter your name: ");
            interrupt = StdinService.ReadInputWithShortcut(out name);
        } while (interrupt == Interrupt.Empty);
        HandleInterrupt(interrupt);

        do
        {
            Console.Write("Enter your email: ");
            interrupt = StdinService.ReadInputWithShortcut(out email);
        } while (interrupt == Interrupt.Empty);
        HandleInterrupt(interrupt);

        do
        {
            Console.Write("Enter your number: ");
            interrupt = StdinService.ReadInputWithShortcut(out number);
        } while (interrupt == Interrupt.Empty);
        HandleInterrupt(interrupt);

        Console.WriteLine("Select your shift:");
        foreach (var shiftOption in Enum.GetValues(typeof(Shift)))
        {
            Console.WriteLine($"{(int)shiftOption} - {shiftOption}");
        }
        do
        {
            interrupt = StdinService.ReadInputWithShortcut(out option);
        } while (interrupt == Interrupt.Empty || !Enum.IsDefined(typeof(Shift), int.Parse(option)));
        HandleInterrupt(interrupt);
        Shift shift = (Shift)int.Parse(option);

        return new Account(username, password, name, email, number, Shift.Morning);
    }
    internal Doctor ReadDoctor()
    {
        string option;
        Interrupt interrupt;
        Account account = ReadAccount();

        Console.Write("Enter your department: ");
        Console.WriteLine("Select your department:");
        foreach (var dept in Enum.GetValues(typeof(Department)))
        {
            Console.WriteLine($"{(int)dept} - {dept}");
        }
        do
        {
            interrupt = StdinService.ReadInputWithShortcut(out option);
        } while (interrupt == Interrupt.Empty || !Enum.IsDefined(typeof(Department), int.Parse(option)));
        HandleInterrupt(interrupt);
        Department department = (Department)int.Parse(option);

        Console.WriteLine("Enter your working days (comma separated):");
        foreach (var day in Enum.GetValues(typeof(DayOfWeek)))
        {
            Console.WriteLine($"{(int)day} - {day}");
        }
        HashSet<DayOfWeek> workingDays = new();
        interrupt = StdinService.ReadInputWithShortcut(out string daysInput);
        HandleInterrupt(interrupt);
        foreach (string day in daysInput.Split(","))
        {
            workingDays.Add((DayOfWeek)int.Parse(day));
        }

        return new Doctor(account, department, workingDays, Auth.Partial);
    }

    internal Assistant ReadAssistant()
    {
        Account account = ReadAccount();
        
        return new Assistant(account);
    }

    internal void PushToStack(Action action)
    {
        actions.Push(action);
    }

    internal void PopFromStack()
    {
        if (actions.Count > 0)
            actions.Pop();
        
        if (actions.Count > 0)
            actions.Peek().Invoke();
        else
            Run();
    }
}

