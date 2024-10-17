namespace ClinicSystem;

using System.Reflection.Metadata;
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

    internal void Run(bool save=false)
    {
        if (save)
            PushToStack(() => Run());
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
            if (interrupt == Interrupt.Empty)
                continue;
            HandleInterrupt(interrupt);
        } while (!int.TryParse(option, out _)); 

        switch (option)
        {
            case "1":
                Login(true, true);
                break;
            case "2":
                Register(true);
                break;
            case "3":
                SwitchSession(true);
                break;
            case "4":
                Environment.Exit(0);
                break;
            default:
                StdinService.Decorate("Invalid option", ConsoleColor.DarkYellow);
                HandleInterrupt(StdinService.ReadKey());
                Run();
                break;
        }
    }

    internal void Login(bool force=false, bool save=false)
    {
        if (save)
            PushToStack(() => Login(force));

        Console.Clear();
        StdinService.Decorate("Login Screen", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "\n\n");
        Session? session = Session.GetCurrentSession();
        if (force || session is null || !Authorizer.checkAuthorized(session))
        {
            Session? newSession;
            int attempts = 0;
            do
            {
                ReadLogin(out string username, out string password, out bool isRemembered);
                newSession = MemoryStorage.Instance.GetSessionByUsername(username);
                if (newSession is not null)
                {
                    if (newSession.Login(password))
                        newSession.IsRemembered = isRemembered;
                    else
                        newSession = null;
                }
                else
                    newSession = Session.CreateSession(username, password, isRemembered);
                attempts++;
            } while (newSession is null && attempts < 3);
            if (newSession is null)
            {
                StdinService.Decorate("Login failed", ConsoleColor.Red);
                HandleInterrupt(StdinService.ReadKey());
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
                Run();
            }
            else
            {
                StdinService.Decorate("Login failed", ConsoleColor.Red);
                MemoryStorage.Instance.CurrentSessionToken = Guid.Empty;
                HandleInterrupt(StdinService.ReadKey());
                Run();
            }
        }

        Thread.Sleep(333);
        MainMenu(true);
    }

    internal void Register(bool save=false)
    {
        if (save)
            PushToStack(() => Register());

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
            if (interrupt == Interrupt.Empty)
                continue;
            HandleInterrupt(interrupt);
        } while (!int.TryParse(option, out _));

        switch (option)
        {
            case "1":
                RegisterDoctor(true);
                break;
            case "2":
                RegisterAssistant(true);
                break;
            default:
                StdinService.Decorate("Invalid option", ConsoleColor.DarkYellow);
                HandleInterrupt(StdinService.ReadKey());
                Register();
                break;
        }

        Login(force: true);
    }

    internal void SwitchSession(bool save=false)
    {
        if (save)
            PushToStack(() => SwitchSession());

        string option;
        Interrupt interrupt;
        Session? session = Session.GetCurrentSession();
        
        Console.Clear();
        StdinService.Decorate("Switch User", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "\n\n");

        List<Session> sessions = MemoryStorage.Instance.Sessions;
        if (sessions.Count == 0)
        {
            StdinService.Decorate("No Sessions Found", ConsoleColor.Red);
            HandleInterrupt(StdinService.ReadKey());
            Login();
        }
        else
        {
            StdinService.Decorate($"   {Session.HeadView()}", ConsoleColor.DarkBlue);
            for (int i = 0; i < sessions.Count; i++)
            {
                Console.WriteLine($"{i+1, -2}  {sessions[i].View()}");
            }

            Console.WriteLine("\n\n");
            do
            {
                Console.Write("Enter session number to switch or (+) to add other user: ");
                interrupt = StdinService.ReadInputWithShortcut(out option);
                if (interrupt == Interrupt.Empty)
                    continue;
                if (option == "+")
                {
                    Login(true);
                    return;
                }
                HandleInterrupt(interrupt);
            } while (!int.TryParse(option, out _) || int.Parse(option) > sessions.Count || int.Parse(option) < 1);

            if (session is not null)
                session.Logout();
            session = sessions[int.Parse(option) - 1];
            if (session.Login(out interrupt))
                StdinService.Decorate("Session switched", ConsoleColor.DarkGreen);
            else
            {
                StdinService.Decorate("Failed to switch session", ConsoleColor.Red);
                HandleInterrupt(StdinService.ReadKey());
                MemoryStorage.Instance.CurrentSessionToken = Guid.Empty;
                Login(true);
            }
            
            Thread.Sleep(555);
            ReturnHome(true);
        }
    }
    internal void MainMenu(bool save=false)
    {
        if (save)
            PushToStack(() => MainMenu());

        Session? session = Session.GetCurrentSession();
        if (session is null || !Authorizer.checkAuthorized(session))
            Login();
        else
        {
            session.UpdateLastLogin();
            switch (session.Role)
            {
                case Role.Doctor:
                    DoctorMenu();
                    break;
                case Role.Assistant:
                    AssisstantMenu();
                    break;
            }
        }
        MainMenu();
    }

    internal void RegisterDoctor(bool save=false)
    {
        if (save)
            PushToStack(() => RegisterDoctor());

        Console.Clear();
        StdinService.Decorate("Register Doctor Screen", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "\n\n");

        Doctor doctor = ReadDoctor();
        MemoryStorage.Instance.AddDoctor(doctor);
    }

    internal void RegisterAssistant(bool save=false)
    {
        if (save)
            PushToStack(() => RegisterAssistant());

        Console.Clear();
        StdinService.Decorate("Register Assistant Screen", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "\n\n");

        Assistant assistant = ReadAssistant();
        MemoryStorage.Instance.AddAssistant(assistant);
    }

    internal void DoctorMenu()
    {
        string option;
        Interrupt interrupt;

        Session? session = Session.GetCurrentSession();
        if (session is null || !Authorizer.checkAuthorized(session))
            Login();

        Console.Clear();
        StdinService.Decorate("Doctor Menu", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "");
        StdinService.Decorate(session!.Account.Username, ConsoleColor.White, ConsoleColor.DarkMagenta, position: Position.Right, end: "\n\n");

        Console.WriteLine("1. View Schedule");
        Console.WriteLine("2. View Patient");
        Console.WriteLine("3. View appointments");
        Console.WriteLine("4. Approve appointment");
        Console.WriteLine("5. Remove appointment");
        Console.WriteLine("6. View Profile");
        Console.WriteLine("7. Switch User");
        Console.WriteLine("8. Logout");

        do
        {
            Console.Write("Choose an option: ");
            interrupt = StdinService.ReadInputWithShortcut(out option);
            if (interrupt == Interrupt.Empty)
                continue;
            HandleInterrupt(interrupt);
        } while (!int.TryParse(option, out _));

        switch (option)
        {
            case "1":
                ViewSchedule(true);
                break;
            case "2":
                ViewPatient(save: true);
                break;
            case "3":
                ViewAppointments(true);
                break;
            case "4":
                ApproveAppointment(true);
                break;
            case "5":
                RemoveAppointment(true);
                break;
            case "6":
                ViewProfile(true);
                break;
            case "7":
                SwitchSession(true);
                break;
            case "8":
                session?.Logout(full: true);
                Run(true);
                break;
            default:
                StdinService.Decorate("Invalid option", ConsoleColor.DarkYellow);
                DoctorMenu();
                break;
        }
    }

    internal void AssisstantMenu()
    {
        string option;
        Interrupt interrupt;

        Session? session = Session.GetCurrentSession();
        if (session is null || !Authorizer.checkAuthorized(session))
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
        Console.WriteLine("7. Switch User");
        Console.WriteLine("8. Logout");

        do
        {
            Console.Write("Choose an option: ");
            interrupt = StdinService.ReadInputWithShortcut(out option);
            if (interrupt == Interrupt.Empty)
                continue;
            HandleInterrupt(interrupt);
        } while (!int.TryParse(option, out _));

        switch (option)
        {
            case "1":
                ViewAppointments(true);
                break;
            case "2":
                AddAppointment(true);
                break;
            case "3":
                RemoveAppointment(true);
                break;
            case "4":
                RegisterPatient(true);
                break;
            case "5":
                ViewPatient(save: true);
                break;
            case "6":
                ViewProfile(true);
                break;
            case "7":
                SwitchSession(true);
                break;
            case "8":
                session?.Logout(full: true);
                Run(true);
                break;
            default:
                StdinService.Decorate("Invalid option", ConsoleColor.DarkYellow);
                AssisstantMenu();
                break;
        }
    }

    internal void ViewSchedule(bool save=false)
    {
        if (save)
            PushToStack(() => ViewSchedule());
        
        Session? session = Session.GetCurrentSession();
        if (session is null || !Authorizer.checkAuthorized(session))
            Login();
        
        Doctor doctor = session!.Account as Doctor ?? throw new Exception("Session account is not a Doctor.");

        Console.Clear();
        StdinService.Decorate($"Schedule ({doctor.Shift})", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "");
        StdinService.Decorate(session.Account.Username, ConsoleColor.White, ConsoleColor.DarkMagenta, position: Position.Right, end: "\n\n");

        for (int i = 0; i < 7; i++)
        {
            DateOnly date = DateOnly.FromDateTime(DateTime.Now.Date.AddDays(i));
            string day = "";
            switch (i)
            {
                case 0:
                    day = "Today";
                    break;
                case 1:
                    day = "Tomorrow";
                    break;
                default:
                    day = date.DayOfWeek.ToString();
                    break;
            }
            List<Appoiment> schedules = doctor.GetSchedules(date);
            StdinService.Decorate($"{day, -20} | ", ConsoleColor.DarkBlue, end: "");
            int StartPoint = Console.CursorLeft;
            foreach (Appoiment appoiment in schedules)
            {
                Console.CursorLeft = StartPoint;
                StdinService.Decorate("*", ConsoleColor.DarkMagenta, end: "");
                ConsoleColor? color = appoiment.State switch
                {
                    AppoimentState.Approved => ConsoleColor.White,
                    AppoimentState.Pending => ConsoleColor.DarkGray,
                    AppoimentState.Canceled => ConsoleColor.Red,
                    _ => null
                };
                StdinService.Decorate($"{appoiment.Time.ToString("hh:mm tt")} - {appoiment.Patient.Name} - {appoiment.Patient.Number}", color);
            }
            if (schedules.Count == 0)
                Console.WriteLine("No Appoiments");
            if (i < 6)
                StdinService.Decorate("".PadRight(Console.WindowWidth - 1, '='), ConsoleColor.DarkGray);
        }

        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.Write("Press any key to return to the menu ");
        HandleInterrupt(StdinService.ReadKey());
        MainMenu(true);
    }
    internal void ViewAppointments(bool save=false)
    {
        if (save)
            PushToStack(() => ViewAppointments());

        Session? session = Session.GetCurrentSession();
        if (session is null || !Authorizer.checkAuthorized(session))
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
        HandleInterrupt(StdinService.ReadKey());
        MainMenu(true);
    }

    internal void ApproveAppointment(bool save=false)
    {
        if (save)
            PushToStack(() => ApproveAppointment());

        string option;
        Interrupt interrupt;

        Session? session = Session.GetCurrentSession();
        if (session is null || !Authorizer.checkAuthorized(session))
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
            if (interrupt == Interrupt.Empty)
                continue;
            HandleInterrupt(interrupt);
        } while (!int.TryParse(option, out _) || int.Parse(option) > MemoryStorage.Instance.Appoiments.Count || int.Parse(option) < 1);

        int idx = int.Parse(option) - 1;
        Appoiment appoiment = MemoryStorage.Instance.Appoiments[idx];

        appoiment.State = AppoimentState.Approved;

        ApproveAppointment();
    }

    internal void RemoveAppointment(bool save=false)
    {
        if (save)
            PushToStack(() => RemoveAppointment());

        string option;
        Interrupt interrupt;

        Session? session = Session.GetCurrentSession();
        if (session is null || !Authorizer.checkAuthorized(session))
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
            if (interrupt == Interrupt.Empty)
                continue;
            HandleInterrupt(interrupt);
        } while (!int.TryParse(option, out _) || int.Parse(option) > MemoryStorage.Instance.Appoiments.Count || int.Parse(option) < 1);

        int idx = int.Parse(option) - 1;
        Appoiment appoiment = MemoryStorage.Instance.Appoiments[idx];

        appoiment.State = AppoimentState.Canceled;
        
        RemoveAppointment();
    }

    internal void AddAppointment(bool save=false)
    {
        if (save)
            PushToStack(() => AddAppointment());

        string option;
        Interrupt interrupt;

        Session? session = Session.GetCurrentSession();
        if (session is null || !Authorizer.checkAuthorized(session))
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

        StdinService.Decorate($"{doctor.DisplayInfo()}", ConsoleColor.DarkMagenta, position: Position.Right, end: "\n\n");

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
        if (attempts == 10)
        {
            Console.WriteLine();
            StdinService.Decorate("Failed to add appointment", ConsoleColor.Red);
            HandleInterrupt(StdinService.ReadKey());
            MainMenu(true);
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
            StdinService.Decorate("Appointment Canceled", ConsoleColor.Red);
        else
        {
            MemoryStorage.Instance.AddAppoiment(appoiment);
            StdinService.Decorate("Appointment Added", ConsoleColor.DarkGreen);
        }
        HandleInterrupt(StdinService.ReadKey());
        if (option == "n")
            MainMenu(true);
        else
            ViewAppointments(true);
    }

    internal void RegisterPatient(bool save=false)
    {
        if (save)
            PushToStack(() => RegisterPatient());

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
        
        HandleInterrupt(StdinService.ReadKey());
        ViewPatient(patient, true);
    }

    internal void ViewPatient(Patient? patient=null, bool save=false)
    {
        if (save)
            PushToStack(() => ViewPatient());


        Console.Clear();
        StdinService.Decorate("View Patient", ConsoleColor.White, ConsoleColor.DarkBlue, position: Position.Center, end: "\n\n");

        if (patient is null)
        {
            patient = ChoosePatient(out string number);
        }
        if (patient is null)
        {
            StdinService.Decorate("Patient not found", ConsoleColor.White, ConsoleColor.Red);
            HandleInterrupt(StdinService.ReadKey());
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
            foreach (Appoiment appoiment in patient.History)
                Console.WriteLine($"\t{appoiment.View()}");


            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("Press any key to return to the menu ");
            HandleInterrupt(StdinService.ReadKey());
            MainMenu(true);
        }
    }

    internal void ViewProfile(bool save=false)
    {
        if (save)
            PushToStack(() => ViewProfile());

        Session? session = Session.GetCurrentSession();
        if (session is null || !Authorizer.checkAuthorized(session))
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
        HandleInterrupt(StdinService.ReadKey());
        MainMenu(true);
    }
    
    internal void ReturnHome(bool save=false)
    {
        Session? session = Session.GetCurrentSession();
        if (session is null || !Authorizer.checkAuthorized(session))
            Run(save);
        else
            MainMenu(save);
    }
    internal Patient? ChoosePatient(out string number)
    {
        Interrupt interrupt;

        do
        {
            Console.Write("Enter patient Number: ");
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
            if (interrupt == Interrupt.Empty)
                continue;
            HandleInterrupt(interrupt);
        } while (!int.TryParse(option, out _) || !Enum.IsDefined(typeof(Department), int.Parse(option)));

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
            HandleInterrupt(StdinService.ReadKey());
            HandleInterrupt(Interrupt.Back);
        }
        do
        {
            Console.Write("Choose a doctor: ");
            interrupt = StdinService.ReadInputWithShortcut(out option);
            if (interrupt == Interrupt.Empty)
                continue;
            HandleInterrupt(interrupt);
        } while (!int.TryParse(option, out _) || !map.ContainsKey(int.Parse(option)));

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
        if (doctor is null)
        {
            List<Appoiment> allAppoiments = MemoryStorage.Instance.Appoiments;
            if (allAppoiments.Count == 0)
                Console.WriteLine("No Appoiments Found");
            else
                StdinService.Decorate($"   {Appoiment.HeadView()}", ConsoleColor.DarkBlue);
            for (int i = 0; i < allAppoiments.Count; i++)
            {
                Console.WriteLine($"{i+1, -2}  {allAppoiments[i].View()}");
            }
        }
        else
        {
            List<Appoiment> appoiments = MemoryStorage.Instance.GetAppoimentsByDoctor(doctor);
            if (appoiments.Count == 0)
                Console.WriteLine("No Appoiments Found");
            else
                StdinService.Decorate($"   {Appoiment.HeadView()}", ConsoleColor.DarkBlue);
            for (int i = 0; i < appoiments.Count; i++)
            {
                Console.WriteLine($"{i+1, -2}  {appoiments[i].View()}");
            }
        }
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
                    ReturnHome(true);
                break;
            case Interrupt.Back:
                if (Actions.Count > 0)
                    PopFromStack(true);
                else
                    ReturnHome(true);
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

        foreach (var shiftOption in Enum.GetValues(typeof(Shift)))
        {
            Console.WriteLine($"{(int)shiftOption} - {shiftOption}");
        }
        do
        {
            Console.Write("Select your shift:");
            interrupt = StdinService.ReadInputWithShortcut(out option);
            if (interrupt == Interrupt.Empty)
                continue;
            HandleInterrupt(interrupt);
        } while (!int.TryParse(option, out _) || !Enum.IsDefined(typeof(Shift), int.Parse(option)));
        Shift shift = (Shift)int.Parse(option);

        return new Account(username, password, name, email, number, shift);
    }
    internal Doctor ReadDoctor()
    {
        string option;
        Interrupt interrupt;
        Account account = ReadAccount();

        foreach (var dept in Enum.GetValues(typeof(Department)))
        {
            Console.WriteLine($"{(int)dept} - {dept}");
        }
        do
        {
            Console.Write("Select your department:");
            interrupt = StdinService.ReadInputWithShortcut(out option);
            if (interrupt == Interrupt.Empty)
                continue;
            HandleInterrupt(interrupt);
        } while (!int.TryParse(option, out _) || !Enum.IsDefined(typeof(Department), int.Parse(option)));

        Department department = (Department)int.Parse(option);

        foreach (var day in Enum.GetValues(typeof(DayOfWeek)))
        {
            Console.WriteLine($"{(int)day + 1} - {day}");
        }
        HashSet<DayOfWeek> workingDays = new();
        do
        {
            Console.WriteLine("Enter your working days (comma separated):");
            interrupt = StdinService.ReadInputWithShortcut(out string daysInput);
            HandleInterrupt(interrupt);
            try
            {
                foreach (string day in daysInput.Split(","))
                {
                    workingDays.Add((DayOfWeek)int.Parse(day) - 1);
                }
            }
            catch (Exception)
            {
                StdinService.Decorate("Invalid input", ConsoleColor.Red);
                continue;
            }
        } while (interrupt == Interrupt.Empty || workingDays.Count == 0);

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
            if (interrupt == Interrupt.Empty)
                continue;
            HandleInterrupt(interrupt);
        } while (!int.TryParse(option, out _) || !Enum.IsDefined(typeof(Gendre), int.Parse(option)));

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
        {
            ReturnHome(true);
        }
    }
}
