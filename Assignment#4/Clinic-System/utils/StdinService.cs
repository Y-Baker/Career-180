namespace ClinicSystem;
public class StdinService
{
    public static void Decorate(string message, ConsoleColor background = ConsoleColor.DarkBlue, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.BackgroundColor = background;
        Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, Console.CursorTop);

        Console.WriteLine($" {message} \n");

        Console.ResetColor();
    }
    public static Interrupt ReadInputWithShortcut(out string input)
    {
        input = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
            {
                input = "";
                return Interrupt.Back;
            }
            else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input = input[0..^1];
                Console.Write("\b \b");
            }
            else if (key.Key != ConsoleKey.Enter)
            {
                input += key.KeyChar;
                Console.Write(key.KeyChar);
            }
        } while (key.Key != ConsoleKey.Enter);
        Console.WriteLine();
        if (input == "")
        {
            return Interrupt.Empty;
        }
        return Interrupt.Success;
    }

    public static Interrupt ReadPassword(out string password)
    {
        password = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
            {
                password = "";
                return Interrupt.Back;
            }
            else if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[0..^1];
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);
        Console.WriteLine();
        if (password == "")
        {
            return Interrupt.Empty;
        }
        return Interrupt.Success;
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
