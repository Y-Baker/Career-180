namespace Assignment_3;

public class Movie
{
    public Dealer Dealer { get; set; }
    public string Title { get; set; }
    public FilmSeries? FilmSeries { get; set; }
    public string Genre { get; set; }
    public string? Description { get; set; }
    public double PricePerDay { get; set; }
    public Subscription SubscriptionLevel { get; set; }
    public int ReleaseYear { get; set; }
    public TimeSpan Duration { get; set; }
    public bool IsAvailable { get; set; }
    public int AgeRating { get; set; }
    public double Rating { get; set; }
    public string? Country { get; set; }
    public List<string>? Languages { get; set; }
    public List<string>? Subtitles { get; set; }
    public List<string>? Cast { get; set; }

    public Movie(Dealer dealer, string title, string genre, double pricePerDay, Subscription subscriptionLevel, int releaseYear, TimeSpan duration, int ageRating, double rating)
    {
        Dealer = dealer;
        Title = title;
        Genre = genre;
        PricePerDay = pricePerDay;
        SubscriptionLevel = subscriptionLevel;
        ReleaseYear = releaseYear;
        Duration = duration;
        AgeRating = ageRating;
        Rating = rating;
        IsAvailable = true;
    }

    public double? CalcPrice(int days, Subscription subscription)
    {
        if (subscription < SubscriptionLevel)
        {
            return null;
        }
        switch (subscription)
        {
            case Subscription.Free:
                return PricePerDay * days;
            case Subscription.Prmium:
                // first week is 50% off
                if (days <= 7)
                    return PricePerDay * days * 0.5;
                return PricePerDay * 7 * 0.5 + PricePerDay * (days - 7);
            case Subscription.PremiumPlus:
                // first week is free
                if (days <= 7)
                    return 0;
                return PricePerDay * (days - 7);
            default:
                return null;
        }
    }

    public void Remove()
    {
        IsAvailable = false;
    }
}
