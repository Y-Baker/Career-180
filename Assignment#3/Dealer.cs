using System.Globalization;

namespace Assignment_3;

public class Dealer : User
{
    public static float ReturnRate = 0.7f;

    public List<Movie> Movies { get; set; }
    public int TotalAdded;
    public int TotalRents;
    public double TotalEarnings;

    public Dealer(string name, string email, string password, string country, DateTime birthdate, string mainLanguage)
        : base(name, email, password, country, birthdate, mainLanguage)
    {
        Movies = new List<Movie>();
        TotalAdded = 0;
        TotalRents = 0;
        TotalEarnings = 0;
    }
    public Dealer(string name, string email, string password, DateTime birthdate)
        : base(name, email, password, birthdate)
    {
        Movies = new List<Movie>();
        TotalAdded = 0;
        TotalRents = 0;
        TotalEarnings = 0;
    }

    public void AddMovie(Movie movie)
    {
        Movies.Add(movie);
        TotalAdded++;
    }
    public void AddMovie()
    {
        StdinService.ReadMovie(this, out Movie movie);
        TotalAdded++;
    }
    public double Withdraw(double amount)
    {
        if (amount < 0)
        {
            Console.WriteLine("Invalid amount");
            return 0;
        }
        if (amount > _wallet)
        {
            Console.WriteLine("Not enough funds");
            return 0;
        }
        _wallet -= amount;
        return amount;
    }

    public void AddOutcomes(Rental rental)
    {
        TotalRents++;
        double earnings = rental.Checkout * ReturnRate;
        TotalEarnings += earnings;
        _wallet += earnings;
    }
}
