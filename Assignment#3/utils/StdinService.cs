namespace Assignment_3;

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

    public static void ReadMovie(Dealer dealer, out Movie movie)
    {
        Console.Write("Enter the title: ");
        string title = Console.ReadLine()!;

        Console.Write("Enter the genre: ");
        string genre = Console.ReadLine()!;
    
        Console.Write("Enter the price Per Day: ");
        double price = double.Parse(Console.ReadLine()!);

        Console.Write("Enter the Subscription Level: ");
        Subscription subscriptionLevel = (Subscription)Enum.Parse(typeof(Subscription), Console.ReadLine()!);

        Console.Write("Enter the Release Year: ");
        int releaseYear = int.Parse(Console.ReadLine()!);

        Console.Write("Enter the Duration: ");
        TimeSpan duration = TimeSpan.Parse(Console.ReadLine()!);

        Console.Write("Enter the Age Rating: ");
        int ageRating = int.Parse(Console.ReadLine()!);

        Console.Write("Enter the rating: ");
        double rating = double.Parse(Console.ReadLine()!);

        movie = new Movie(dealer, title, genre, price, subscriptionLevel, releaseYear, duration, ageRating, rating);
    }
}
