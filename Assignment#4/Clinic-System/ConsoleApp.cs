namespace ClinicSystem;

using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;

internal class ConsoleApp
{
    private Stack<Action> Actions = new Stack<Action>();
    private readonly IConfiguration? Config;

    internal ConsoleApp(Doctor? admin=null, IConfiguration? config=null)
    {
        if (admin is not null)
            MemoryStorage.Instance.AddDoctor(admin);
        Config = config;
        IConfigurationSection? workingHours = Config?.GetSection("WorkingHours");
        if (workingHours is not null)
            MemoryStorage.Instance.SetWorkingHours(workingHours);
    }
    internal void Run()
    {
        string option;
        Interrupt interrupt;

        Console.Clear();
        StdinService.Decorate("Welcome to Clinic System!", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "\n\n");

        Console.WriteLine("1. Login");
        Console.WriteLine("2. Register");
        Console.WriteLine("3. Switch User");
        Console.WriteLine("4. Exit");
        Console.WriteLine();

        do
        {
            Console.Write("Choose an option: ");
            interrupt = StdinService.ReadInputWithShortcut(out option);
        } while (interrupt == Interrupt.Empty);
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
                Console.ReadKey();
                Run();
                break;
        }
    }


    internal void Login()
    {
        Console.Clear();
        StdinService.Decorate("Login Screen", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "\n\n");
        Session? session = Session.GetCurrentSession();
        if (session is null)
        {
            Session? newSession;
            int attempts = 0;
            do
            {
                ReadLogin(out string username, out string password, out bool isRemembered);
                // todo: add session to the memory storage and don't create a new one if it's already exists
                newSession = Session.CreateSession(username, password, isRemembered);
                attempts++;
            } while (newSession is null && attempts < 3);
            if (newSession is null)
            {
                StdinService.Decorate("Login failed", ConsoleColor.Red);
                Console.ReadKey();
                PushToStack(Run);
                Run();
            } 
            else
                StdinService.Decorate("Login successful", ConsoleColor.DarkGreen);
        }
        else
        {
            if (session.Login(out Interrupt interrupt))
                Console.WriteLine("Login successful");
            else if (interrupt == Interrupt.Back)
            {
                MemoryStorage.Instance.CurrentSessionToken = Guid.Empty;
                PushToStack(Run);
                Run();
            }
            else
            {
                StdinService.Decorate("Login failed", ConsoleColor.Red);
                MemoryStorage.Instance.CurrentSessionToken = Guid.Empty;
                Console.ReadKey();
                PushToStack(Run);
                Run();
            }
        }

        PushToStack(MainMenu);
        MainMenu();
    }

    internal void Register()
    {
        string option;
        Interrupt interrupt;

        Console.Clear();
        StdinService.Decorate("Register Screen", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "\n\n");

        Console.WriteLine("1. Doctor");
        Console.WriteLine("2. Assistant");
        Console.WriteLine();

        do
        {
            Console.Write("Choose an option: ");
            interrupt = StdinService.ReadInputWithShortcut(out option);
        } while (interrupt == Interrupt.Empty);
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
                StdinService.Decorate("Invalid option", ConsoleColor.DarkYellow);
                Console.ReadKey();
                Register();
                break;
        }

        PushToStack(Login);
        Login();
    }

    internal void MainMenu()
    {
        Session? session = Session.GetCurrentSession();
        if (session is null || session.IsLoggedIn == false)
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
        StdinService.Decorate("Register Doctor Screen", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "\n\n");

        Doctor doctor = ReadDoctor();
        MemoryStorage.Instance.AddDoctor(doctor);
    }

    internal void RegisterAssistant()
    {
        Console.Clear();
        StdinService.Decorate("Register Assistant Screen", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "\n\n");

        Assistant assistant = ReadAssistant();
        MemoryStorage.Instance.AddAssistant(assistant);
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

    internal void DoctorMenu()
    {
        string option;
        Interrupt interrupt;

        Session? session = Session.GetCurrentSession();
        if (session is null || session.IsLoggedIn == false)
            Login();

        Console.Clear();
        StdinService.Decorate("Doctor Menu", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "");
        StdinService.Decorate(session!.Account.Username, ConsoleColor.White, ConsoleColor.DarkMagenta, position: Position.Right, end: "\n\n");

        Console.WriteLine("1. View appointments");
        Console.WriteLine("2. Approve appointment");
        Console.WriteLine("3. Remove appointment");
        Console.WriteLine("4. View Profile");
        Console.WriteLine("5. Logout");

        do
        {
            Console.Write("Choose an option: ");
            interrupt = StdinService.ReadInputWithShortcut(out option);
        } while (interrupt == Interrupt.Empty);
        HandleInterrupt(interrupt);

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
                PushToStack(ViewProfile);
                ViewProfile();
                break;
            case "5":
                session?.Logout(full: true);
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

        Session? session = Session.GetCurrentSession();
        if (session is null || session.IsLoggedIn == false)
            Login();

        Console.Clear();
        StdinService.Decorate("Assistant Menu", position: Position.Center, end: "");
        StdinService.Decorate(session!.Account.Username, ConsoleColor.White, ConsoleColor.DarkMagenta, position: Position.Right, end: "\n\n");

        Console.WriteLine("1. View appointments");
        Console.WriteLine("2. Add appointment");
        Console.WriteLine("3. Remove appointment");
        Console.WriteLine("4. Register Patient");
        Console.WriteLine("5. View Patient");
        Console.WriteLine("6. View Profile");
        Console.WriteLine("7. Logout");

        do
        {
            Console.Write("Choose an option: ");
            interrupt = StdinService.ReadInputWithShortcut(out option);
        } while (interrupt == Interrupt.Empty);
        HandleInterrupt(interrupt);

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
                PushToStack(RegisterPatient);
                RegisterPatient();
                break;
            case "5":
                ViewPatient();
                break;
            case "6":
                PushToStack(ViewProfile);
                ViewProfile();
                break;
            case "7":
                session?.Logout(full: true);
                PushToStack(Run);
                Run();
                break;
            default:
                Console.WriteLine("Invalid option!");
                AssisstantMenu();
                break;
        }
    }

    internal void ViewAppointments()
    {
        Session? session = Session.GetCurrentSession();
        if (session is null || session.IsLoggedIn == false)
            Login();

        Console.Clear();
        StdinService.Decorate("Appointments", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "");
        StdinService.Decorate(session!.Account.Username, ConsoleColor.White, ConsoleColor.DarkMagenta, position: Position.Right, end: "\n\n");

        if (session!.Role == Role.Doctor)
        {
            Doctor doctor = session.Account as Doctor ?? throw new Exception("Session account is not a Doctor.");
            ListAppointments(doctor);
        }
        else
            ListAppointments();

        // TODO: Add a way to view a single appointment
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.Write("Press any key to return to the menu ");
        Console.ReadKey();
        ReturnToMenu();
    }

    internal void ApproveAppointment()
    {
        string option;
        Interrupt interrupt;

        Session? session = Session.GetCurrentSession();
        if (session is null || session.IsLoggedIn == false)
            Login();

        Console.Clear();
        StdinService.Decorate("Approve Appointment", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "");
        StdinService.Decorate(session!.Account.Username, ConsoleColor.White, ConsoleColor.DarkMagenta, position: Position.Right, end: "\n\n");

        ListAppointments(session!.Account as Doctor);
        Console.WriteLine();
        do
        {
            Console.Write("Enter appointment number to approve: ");
            interrupt = StdinService.ReadInputWithShortcut(out option);
        } while (interrupt == Interrupt.Empty);
        HandleInterrupt(interrupt);

        int idx = int.Parse(option) - 1;
        Appoiment appoiment = MemoryStorage.Instance.Appoiments[idx];
        appoiment.State = AppoimentState.Approved;
        ApproveAppointment();
    }

    internal void RemoveAppointment()
    {
        string option;
        Interrupt interrupt;

        Session? session = Session.GetCurrentSession();
        if (session is null || session.IsLoggedIn == false)
            Login();

        Console.Clear();
        StdinService.Decorate("Remove Appointment", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "");
        StdinService.Decorate(session!.Account.Username, ConsoleColor.White, ConsoleColor.DarkMagenta, position: Position.Right, end: "\n\n");

        ListAppointments(session!.Account as Doctor);
        Console.WriteLine();

        do
        {
            Console.Write("Enter appointment number to remove: ");
            interrupt = StdinService.ReadInputWithShortcut(out option);
        } while (interrupt == Interrupt.Empty);
        HandleInterrupt(interrupt);

        int idx = int.Parse(option) - 1;
        Appoiment appoiment = MemoryStorage.Instance.Appoiments[idx];
        appoiment.State = AppoimentState.Cancelled;
    }

    internal void AddAppointment()
    {
        string option;
        Interrupt interrupt;

        Session? session = Session.GetCurrentSession();
        if (session is null || session.IsLoggedIn == false)
            Login();

        Console.Clear();
        StdinService.Decorate("Add Appointment", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "");
        StdinService.Decorate(session!.Account.Username, ConsoleColor.White, ConsoleColor.DarkMagenta, position: Position.Right, end: "\n\n");

        Department department = ChooseDepartment();
        Doctor doctor = ChooseDoctor(department);
        
        Patient? patient = ChoosePatient(out string number);
        if (patient is null)
        {
            Console.WriteLine();
            StdinService.Decorate("Create a new patient profile", background: ConsoleColor.DarkBlue, position: Position.Center);
            patient = ReadPatient(number);
            MemoryStorage.Instance.AddPatient(patient);
            StdinService.Decorate("Patient profile created", ConsoleColor.DarkGreen, position: Position.Left, end: "\n\n");
        }

        DateTime time;
        int attempts = 0;
        do
        {
            StdinService.Decorate("Enter Appointment time", ConsoleColor.DarkYellow, position: Position.Center, end: "\n\n");
            interrupt = StdinService.ReadTime(out time);
            if (interrupt == Interrupt.Exit)
                continue;
            if (interrupt == Interrupt.Back)
                break;
            attempts++;
        } while (!ValidTime(time, doctor) && attempts < 10);
        if (attempts == 3)
        {
            Console.WriteLine();
            StdinService.Decorate("Failed to add appointment", ConsoleColor.Red);
            Console.ReadKey();
            ReturnToMenu();
        }
        HandleInterrupt(interrupt);

        IConfigurationSection? priceConfig = Config?.GetSection("Price");
        double price = double.Parse(priceConfig?[department.ToString()] ?? "100");
        
        Assistant assistant = session!.Account as Assistant ?? throw new Exception("Session account is not an Assistant.");
        Appoiment appoiment = new Appoiment(time, patient, assistant, doctor, price);

        do
        {
            StdinService.Decorate($"Confirm Appointment (Price: {price}) (y/n):",background: ConsoleColor.DarkYellow, end: "");
            Console.Write(" ");
            option = Console.ReadLine()!.ToLower();
        } while (option != "y" && option != "n");
        if (option == "n")
            StdinService.Decorate("Appointment Cancelled", ConsoleColor.Red);
        else
        {
            MemoryStorage.Instance.AddAppoiment(appoiment);
            StdinService.Decorate("Appointment Added", ConsoleColor.DarkGreen);
        }
        Console.ReadKey();
        if (option == "n")
        {
            PushToStack(ReturnToMenu);
            ReturnToMenu();
        }
        else
        {
            PushToStack(ViewAppointments);
            ViewAppointments();
        }
    }

    internal void RegisterPatient()
    {
        Console.Clear();
        StdinService.Decorate("Register Patient", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "\n\n");

        Patient? patient = ChoosePatient(out string number);
        if (patient is null)
        {
            patient = ReadPatient(number);
            MemoryStorage.Instance.AddPatient(patient);
            StdinService.Decorate("Patient profile created", background: ConsoleColor.DarkGreen, position: Position.Center);
        }
        else
            StdinService.Decorate("Patient already exists", background: ConsoleColor.Red, position: Position.Center);
        
        Console.ReadKey();
        ViewPatient(patient);
    }

    internal void ViewPatient(Patient? patient=null)
    {
        Interrupt interrupt;

        Console.Clear();
        StdinService.Decorate("View Patient", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "\n\n");

        if (patient is null)
        {
            patient = ChoosePatient(out string number);
        }
        if (patient is null)
        {
            StdinService.Decorate("Patient not found", background: ConsoleColor.Red);
            Console.ReadKey();
            ViewPatient();
        }
        else
        {
            StdinService.Decorate("Name: ", ConsoleColor.DarkBlue, end: "");
            Console.WriteLine(patient.Name);

            StdinService.Decorate("Number: ", ConsoleColor.DarkBlue, end: "");
            Console.WriteLine(patient.Number);

            StdinService.Decorate("Address: ", ConsoleColor.DarkBlue, end: "");
            Console.WriteLine(patient.Address);

            StdinService.Decorate("Age: ", ConsoleColor.DarkBlue, end: "");
            Console.WriteLine(patient.Age);

            StdinService.Decorate("Gendre: ", ConsoleColor.DarkBlue, end: "");
            Console.WriteLine(patient.Gendre);

            StdinService.Decorate("History: ", ConsoleColor.DarkBlue, end: "\n");
            foreach (var appoiment in patient.History)
                Console.WriteLine($"\t{ViewAppointment(appoiment)}");

            do
            {
                interrupt = StdinService.ReadInputWithShortcut(out _, true);
            } while (interrupt != Interrupt.Back);
            HandleInterrupt(interrupt);

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("Press any key to return to the menu ");
            Console.ReadKey();
            ReturnToMenu();
        }
    }

    internal void ViewProfile()
    {
        Session? session = Session.GetCurrentSession();
        if (session is null || session.IsLoggedIn == false)
            Login();

        Console.Clear();
        StdinService.Decorate("Profile", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "");
        StdinService.Decorate(session!.Account.Username, ConsoleColor.White, ConsoleColor.DarkMagenta, position: Position.Right, end: "\n\n");

        StdinService.Decorate("Name: ", ConsoleColor.DarkBlue, end: "");
        Console.WriteLine(session.Account.Name);

        StdinService.Decorate("Role: ", ConsoleColor.DarkBlue, end: "");
        Console.WriteLine(session.Role);

        StdinService.Decorate("Email: ", ConsoleColor.DarkBlue, end: "");
        Console.WriteLine(session.Account.Email);

        StdinService.Decorate("Number: ", ConsoleColor.DarkBlue, end: "");
        Console.WriteLine(session.Account.Number);

        StdinService.Decorate("Shift: ", ConsoleColor.DarkBlue, end: "");
        Console.WriteLine(session.Account.Shift);

        if (session.Role == Role.Doctor)
        {
            Doctor doctor = session.Account as Doctor ?? throw new Exception("Session account is not a Doctor.");
            StdinService.Decorate("Department: ", ConsoleColor.DarkBlue, end: "");
            Console.WriteLine(doctor.Department);

            StdinService.Decorate("Working Days: ", ConsoleColor.DarkBlue, end: "");
            foreach (var day in doctor.WorkingDays)
                Console.Write($"{day}, ");
            Console.WriteLine();

            StdinService.Decorate("Access: ", ConsoleColor.DarkBlue, end: "");
            Console.WriteLine(doctor.Auth);
        }

        StdinService.Decorate("Last Login: ", ConsoleColor.DarkBlue, end: "");
        Console.WriteLine(session.LoginTime);

        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.Write("Press any key to return to the menu ");
        Console.ReadKey();
        ReturnToMenu();
    }
    internal Patient? ChoosePatient(out string number)
    {
        Interrupt interrupt;

        do
        {
            Console.WriteLine("Enter patient Number: ");
            interrupt = StdinService.ReadInputWithShortcut(out number);
        } while (interrupt == Interrupt.Empty);
        HandleInterrupt(interrupt);
        
        Patient? patient = MemoryStorage.Instance.GetPatientByNumber(number);
        return patient;
    }

    internal Department ChooseDepartment()
    {
        string option;
        Interrupt interrupt;

        ListDepartments();
        do
        {
            Console.Write("Choose a department: ");
            interrupt = StdinService.ReadInputWithShortcut(out option);
            if (interrupt == Interrupt.Back)
                break;
        } while (interrupt == Interrupt.Empty || option == "" || !Enum.IsDefined(typeof(Department), int.Parse(option)));
        HandleInterrupt(interrupt);

        int departmentIdx = int.Parse(option);
        Department department = (Department)departmentIdx;
        return department;
    }
    internal Doctor ChooseDoctor(Department? department=null)
    {
        string option;
        Interrupt interrupt;

        Dictionary<int, Doctor> map = ListDoctors(department);
        if (map.Count == 0)
        {
            StdinService.Decorate("No Doctors Found", ConsoleColor.Red);
            Console.ReadKey();
            HandleInterrupt(Interrupt.Back);
        }
        do
        {
            Console.Write("Choose a doctor: ");
            interrupt = StdinService.ReadInputWithShortcut(out option);
            if (interrupt == Interrupt.Back)
                break;
        } while (interrupt == Interrupt.Empty || !map.ContainsKey(int.Parse(option)));
        HandleInterrupt(interrupt);

        Doctor doctor = map[int.Parse(option)];
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

    internal Dictionary<int, Doctor> ListDoctors(Department? department=null)
    {
        Dictionary<int, Doctor> doctorMap = new();
        if (department == null)
        {
            List<Doctor> doctors = MemoryStorage.Instance.Doctors;
            for (int i = 0; i < doctors.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {doctors[i].Name}");
                doctorMap.Add(i + 1, doctors[i]);
            }
        }
        else
        {
            List<Doctor> doctors = MemoryStorage.Instance.GetDoctorsByDepartment(department ?? Department.General);
            for (int i = 0; i < doctors.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {doctors[i].Name}");
                doctorMap.Add(i + 1, doctors[i]);
            }
        }
        return doctorMap;
    }
    internal void ListAppointments(Doctor? doctor=null)
    {
        Console.WriteLine($"{"Time",-20} {"Patient",-20} {"State",-20} {"Paid",-20} {"Price",-20}");
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
            StdinService.Decorate("Time Cannot be in the past", ConsoleColor.Red, end: "\n\n");
            return false;
        }
        if (time > DateTime.Now.AddMonths(1))
        {
            StdinService.Decorate("Time Cannot be more than a month from now", ConsoleColor.Red, end: "\n\n");
            return false;
        }

        if (!doctor.TimeIsAvailable(time))
        {
            StdinService.Decorate("Doctor is not available at this time", ConsoleColor.Red, end: "\n\n");
            return false;
        }

        if (!Appoiment.TimeIsAvailable(time, doctor.Department))
        {
            StdinService.Decorate("There is an Appoiment at this time", ConsoleColor.Red, end: "\n\n");
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
                if (Actions.Count > 0)
                    Actions.Peek().Invoke();
                else
                    Run();
                break;
            case Interrupt.Back:
                if (Actions.Count > 0)
                    PopFromStack(true);
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
        string option;
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
            Console.Write("Remember me? (y/n): ");
            option = Console.ReadLine()!.ToLower();
        } while (option != "y" && option != "n");
        isRemembered = option == "y";
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
            if (interrupt == Interrupt.Back)
                break;
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

        Console.WriteLine("Select your department:");
        foreach (var dept in Enum.GetValues(typeof(Department)))
        {
            Console.WriteLine($"{(int)dept} - {dept}");
        }
        do
        {
            interrupt = StdinService.ReadInputWithShortcut(out option);
            if (interrupt == Interrupt.Back)
                break;
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

    internal Patient ReadPatient(string? number = null)
    {
        string name, address, age, option;
        Interrupt interrupt;
        do
        {
            Console.Write("Enter your name: ");
            interrupt = StdinService.ReadInputWithShortcut(out name);
        } while (interrupt == Interrupt.Empty);
        HandleInterrupt(interrupt);

        if (number is null)
        {
            do
            {
                Console.Write("Enter your number: ");
                interrupt = StdinService.ReadInputWithShortcut(out number);
            } while (interrupt == Interrupt.Empty);
            HandleInterrupt(interrupt);
        }

        do
        {
            Console.Write("Enter your address: ");
            interrupt = StdinService.ReadInputWithShortcut(out address);
        } while (interrupt == Interrupt.Empty);
        HandleInterrupt(interrupt);

        do
        {
            Console.Write("Enter your age: ");
            interrupt = StdinService.ReadInputWithShortcut(out age);
        } while (interrupt == Interrupt.Empty);
        HandleInterrupt(interrupt);

        foreach (var genderOption in Enum.GetValues(typeof(Gendre)))
        {
            Console.WriteLine($"{(int)genderOption} - {genderOption}");
        }
        do
        {
            Console.Write("Select your gender: ");
            interrupt = StdinService.ReadInputWithShortcut(out option);
            if (interrupt == Interrupt.Back)
                break;
        } while (interrupt == Interrupt.Empty || !Enum.IsDefined(typeof(Gendre), int.Parse(option)));
        HandleInterrupt(interrupt);

        return new Patient(name, number, address, int.Parse(age), (Gendre)int.Parse(option));
    }

    internal void PushToStack(Action action)
    {
        Actions.Push(action);
    }

    internal void PopFromStack(bool invoke = false)
    {
        if (Actions.Count > 0)
            Actions.Pop();

        if (!invoke)
            return;

        if (Actions.Count > 0)
            Actions.Peek().Invoke();
        else
            Run();
    }
}
