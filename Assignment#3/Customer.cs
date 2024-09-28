namespace Assignment_3;

public class Customer : User
{
    public Subscription subscription;
    public List<Movie> Library;
    public List<Customer> FamilyMembers;
    public List<Rental> Rentals;

    public Customer(string name, string email, string password, DateTime birthdate, Subscription subscription)
        : base(name, email, password, birthdate)
    {
        this.subscription = subscription;
        Library = new List<Movie>();
        FamilyMembers = new List<Customer>();
        Rentals = new List<Rental>();
    }
    public Customer(string name, string email, string password, string country, DateTime birthdate, string mainLanguage, Subscription subscription)
        : base(name, email, password, country, birthdate, mainLanguage)
    {
        this.subscription = subscription;
        Library = new List<Movie>();
        FamilyMembers = new List<Customer>();
        Rentals = new List<Rental>();
    }
    public void Deposit(double amount)
    {
        if (amount < 0)
        {
            Console.WriteLine("Invalid amount");
            return;
        }
        _wallet += amount;
    }
    public void RentMovie(Movie movie, int days)
    {
        if (!SessionManger.checkAuthorized(this))
        {
            Console.WriteLine("You are not authorized to rent this movie");
            return;
        }
        if (Library.Contains(movie))
        {
            Console.WriteLine("You already have this movie in your library");
            return;
        }
        if (movie.IsAvailable == false)
        {
            Console.WriteLine("This movie is not available");
            return;
        }
        if (movie.AgeRating > this.getAge())
        {
            Console.WriteLine("You are not old enough to rent this movie");
            return;
        }

        double? price = movie.CalcPrice(days, subscription);
        if (price == null)
        {
            Console.WriteLine("You can't rent this movie");
            return;
        }
        if (_wallet < price)
        {
            Console.WriteLine("Not enough money in your wallet");
            return;
        }
        Rentals.Add(new Rental(this, movie, days, price.Value));
        _wallet -= price.Value;
        Library.Add(movie);
        Console.WriteLine("Movie rented successfully");
    }

    public void ReturnMovie(Movie movie)
    {
        if (!SessionManger.checkAuthorized(this))
        {
            Console.WriteLine("You are not authorized to return this movie");
            return;
        }
        if (!Library.Contains(movie))
        {
            Console.WriteLine("You don't have this movie in your library");
            return;
        }
        Rental rental = Rentals.Find(r => r.Movie == movie) ?? throw new Exception("Rental not found");
        rental.Return(this);
        Library.Remove(movie);
        Console.WriteLine("Movie returned successfully");
        UpdateLibrary();
    }

    public void UpdateLibrary()
    {
        List<Rental> toRemove = new List<Rental>();
        foreach (var rental in Rentals)
        {
            if (DateTime.Now > rental.DateReturned)
            {
                rental.Movie.Dealer.AddOutcomes(rental);
                Library.Remove(rental.Movie);
                toRemove.Add(rental);
            }
        }
    }
}
