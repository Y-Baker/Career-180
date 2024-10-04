namespace ClinicSystem;
public class StdinService
{
    public static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);

            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else
            {
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[0..^1];
                    Console.Write("\b \b");
                }
            }
        } while (key.Key != ConsoleKey.Enter);
        Console.WriteLine();
        return password;
    }

    public static void ReadLogin(out string username, out string password, out bool isRemembered)
    {
        Console.Write("Enter your username: ");
        username = Console.ReadLine()!;
        Console.Write("Enter your password: ");
        password = ReadPassword();
        Console.Write("Remember me? (y/n): ");
        isRemembered = Console.ReadLine()!.ToLower() == "y";
    }

    public static Doctor ReadDoctor()
    {
        Console.Write("Enter your username: ");
        string username = Console.ReadLine()!;
        Console.Write("Enter your password: ");
        string password = ReadPassword();
        Console.Write("Enter your name: ");
        string name = Console.ReadLine()!;
        Console.Write("Enter your email: ");
        string email = Console.ReadLine()!;
        Console.Write("Enter your number: ");
        string number = Console.ReadLine()!;
        Console.Write("Enter your department: ");
        Console.WriteLine("Select your department:");
        foreach (var dept in Enum.GetValues(typeof(Department)))
        {
            Console.WriteLine($"{(int)dept} - {dept}");
        }
        Department department = (Department)int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter your working days (comma separated):");
        foreach (var day in Enum.GetValues(typeof(DayOfWeek)))
        {
            Console.WriteLine($"{(int)day} - {day}");
        }
        HashSet<DayOfWeek> workingDays = new();
        foreach (string day in Console.ReadLine()!.Split(","))
        {
            workingDays.Add((DayOfWeek)int.Parse(day));
        }

        Console.WriteLine("Select your shift:");
        foreach (var shiftOption in Enum.GetValues(typeof(Shift)))
        {
            Console.WriteLine($"{(int)shiftOption} - {shiftOption}");
        }
        Shift shift = (Shift)int.Parse(Console.ReadLine()!);

        return new Doctor(username, password, name, email, number, department, workingDays, Auth.Partial, shift);
    }

    public static Assistant ReadAssistant()
    {
        Console.Write("Enter your username: ");
        string username = Console.ReadLine()!;
        Console.Write("Enter your password: ");
        string password = ReadPassword();
        Console.Write("Enter your name: ");
        string name = Console.ReadLine()!;
        Console.Write("Enter your email: ");
        string email = Console.ReadLine()!;
        Console.Write("Enter your number: ");
        string number = Console.ReadLine()!;
        Console.WriteLine("Select your shift:");
        foreach (var shiftOption in Enum.GetValues(typeof(Shift)))
        {
            Console.WriteLine($"{(int)shiftOption} - {shiftOption}");
        }
        Shift shift = (Shift)int.Parse(Console.ReadLine()!);

        return new Assistant(username, password, name, email, number, shift);
    }

    public static Patient ReadPatient()
    {
        Console.Write("Enter your name: ");
        string name = Console.ReadLine()!;
        Console.Write("Enter your number: ");
        string number = Console.ReadLine()!;
        Console.Write("Enter your address: ");
        string address = Console.ReadLine()!;
        Console.Write("Enter your age: ");
        int age = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter your gender: ");
        foreach (var genderOption in Enum.GetValues(typeof(Gendre)))
        {
            Console.WriteLine($"{(int)genderOption} - {genderOption}");
        }
        Gendre gendre = (Gendre)int.Parse(Console.ReadLine()!);
        return new Patient(name, number, address, age, gendre);
    }

    public static DateTime ReadTime()
    {
        Console.Write("Enter the year: ");
        int year = int.Parse(Console.ReadLine()!);
        Console.Write("Enter the month: ");
        int month = int.Parse(Console.ReadLine()!);
        Console.Write("Enter the day: ");
        int day = int.Parse(Console.ReadLine()!);
        Console.Write("Enter the hour: ");
        int hour = int.Parse(Console.ReadLine()!);
        Console.Write("Enter the minute: ");
        int minute = int.Parse(Console.ReadLine()!);
        return new DateTime(year, month, day, hour, minute, 0);
    }
}
