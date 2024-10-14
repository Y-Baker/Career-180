namespace TimeSpan;

public class Duration
{
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public int Seconds { get; set; }

    public Duration(int hours, int minutes, int seconds)
    {
        Hours = hours;
        Minutes = minutes;
        Seconds = seconds;
    }
    public Duration(int seconds)
    {
        Hours = seconds / (60 * 60);
        seconds -= Hours * (60 * 60);

        Minutes = seconds / 60;
        seconds -= Minutes * 60;

        Seconds -= seconds;
    }
    public string getstring()
    {
        return $"Hours: {Hours}, Minutes :{Minutes} , Seconds :{Seconds}";
    }
}
