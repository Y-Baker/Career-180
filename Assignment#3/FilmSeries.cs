namespace Assignment_3;

public class FilmSeries
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Genre { get; set; }
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }
    public List<Movie>? Movies { get; set; }

    public FilmSeries(string name)
    {
        Name = name;
        Movies = new List<Movie>();
    }
    public void AddMovie(Movie movie)
    {
        Movies?.Add(movie);
        if (StartYear == null)
        {
            StartYear = movie.ReleaseYear;
            EndYear = movie.ReleaseYear;
        }
        else
        {
            if (movie.ReleaseYear < StartYear)
                StartYear = movie.ReleaseYear;
            if (movie.ReleaseYear > EndYear)
                EndYear = movie.ReleaseYear;
        }
    }
}
