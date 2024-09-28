namespace Assignment_3;

public class Rental
{
    public Guid Id { get; set; }
    public DateTime DateRented { get; set; }
    public DateTime DateReturned { get; set; }
    public Customer Customer { get; set; }
    public Movie Movie { get; set; }
    public double Checkout { get; set; }

    public Rental(Customer customer, Movie movie, int days, double checkout)
    {
        Id = Guid.NewGuid();
        DateRented = DateTime.Now;
        DateReturned = DateRented.AddDays(days);
        Customer = customer;
        Movie = movie;
        Checkout = checkout;
    }

    public void Return(Customer customer)
    {
        if (!SessionManger.checkAuthorized(customer))
        {
            Console.WriteLine("You are not authorized to return this movie");
            return;
        }
        if (DateTime.Now > DateReturned)
            return;
        int days = (int)(DateTime.Now - DateRented).TotalDays;
        double returnMoney = Movie.PricePerDay * days * 0.3;
        Checkout -= returnMoney;
        customer.Deposit(returnMoney);
        DateReturned = DateTime.Now;
    }
}
