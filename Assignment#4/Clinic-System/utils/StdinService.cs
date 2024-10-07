namespace ClinicSystem;
public class StdinService
{
    public static void Decorate(string message, ConsoleColor? color = null,
                                ConsoleColor? background = null, Position? position = null, string end="\n")
    {
        if (background is not null)
            Console.BackgroundColor = (ConsoleColor)background;
        if (color is not null)
            Console.ForegroundColor = (ConsoleColor)color;
        switch (position)
        {
            case Position.Center:
                Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, Console.CursorTop);
                break;
            case Position.Left:
                Console.SetCursorPosition(0, Console.CursorTop);
                break;
            case Position.Right:
                Console.SetCursorPosition(Console.WindowWidth - message.Length - 4, Console.CursorTop);
                break;
            case null:
                break;
        }
        Console.Write($" {message} " + end);

        Console.ResetColor();
    }
    public static Interrupt ReadInputWithShortcut(out string input, bool noWrite = false)
    {
        input = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
            {
                input = "*";
                return Interrupt.Back;
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (input.Length == 0)
                    continue;
                input = input[0..^1];
                Console.Write("\b \b");
            }
            else if (key.Key != ConsoleKey.Enter && noWrite == false)
            {
                input += key.KeyChar;
                Console.Write(key.KeyChar);
            }
        } while (key.Key != ConsoleKey.Enter);
        Console.WriteLine();
        if (input == "")
        {
            input = "*";
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

    public static Interrupt ReadTime(out DateTime time)
    {
        time = DateTime.MinValue;
        int year, month, day, hour, minute;
        Interrupt interrupt;
        string input;
        do {
            Console.Write("Enter the year: ");
            interrupt = ReadInputWithShortcut(out input);
            if (interrupt == Interrupt.Back)
                return Interrupt.Back;
        } while (interrupt == Interrupt.Empty || !int.TryParse(input, out year));

        do {
            Console.Write("Enter the month: ");
            interrupt = ReadInputWithShortcut(out input);
            if (interrupt == Interrupt.Back)
                return Interrupt.Back;
        } while (interrupt == Interrupt.Empty || !int.TryParse(input, out month));

        do {
            Console.Write("Enter the day: ");
            interrupt = ReadInputWithShortcut(out input);
            if (interrupt == Interrupt.Back)
                return Interrupt.Back;
        } while (interrupt == Interrupt.Empty || !int.TryParse(input, out day));

        do {
            Console.Write("Enter the hour: ");
            interrupt = ReadInputWithShortcut(out input);
            if (interrupt == Interrupt.Back)
                return Interrupt.Back;
        } while (interrupt == Interrupt.Empty || !int.TryParse(input, out hour));

        do {
            Console.Write("Enter the minute: ");
            interrupt = ReadInputWithShortcut(out input);
            if (interrupt == Interrupt.Back)
                return Interrupt.Back;
        } while (interrupt == Interrupt.Empty || !int.TryParse(input, out minute));

        Console.WriteLine();
        try
        {
            time = new DateTime(year, month, day, hour, minute, 0);
        }
        catch (Exception ex)
        {
            Decorate(ex.Message, ConsoleColor.Red);
            return Interrupt.Exit;
        }
        time = new DateTime(year, month, day, hour, minute, 0);
        return Interrupt.Success;
    }
}
