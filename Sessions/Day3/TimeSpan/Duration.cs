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

    public override string ToString()
    {
        // 11H:12M:25S
        return $"{Hours}H:{Minutes}M:{Seconds}S";
    }

    public int TotalSeconds()
    {
        return Hours * 60 * 60 + Minutes * 60 + Seconds;
    }
    public override bool Equals(object? obj)
    {
        if (obj is Duration duration)
        {
            return Hours == duration.Hours && Minutes == duration.Minutes && Seconds == duration.Seconds;
        }
        return false;
    }

    public static Duration operator +(Duration duration1, Duration duration2)
    {
        int seconds, minutes = 0, hours = 0;
        seconds = duration1.Seconds + duration2.Seconds;
        if (seconds >= 60)
        {
            seconds -= 60;
            minutes++;
        }
        minutes += duration1.Minutes + duration2.Minutes;
        if (minutes >= 60)
        {
            minutes -= 60;
            hours++;
        }
        hours += duration1.Hours + duration2.Hours;
        return new Duration(hours, minutes, seconds);
    }

    public static Duration operator +(int seconds, Duration duration1)
    {
        Duration duration2 = new Duration(seconds);
        return duration1 + duration2;
    }

    public static Duration operator ++(Duration duration)
    {
        Duration tmp = new Duration(60);
        return duration + tmp;
    }

    public static Duration operator --(Duration duration)
    {
        if (duration.Minutes > 0)
        {
            duration.Minutes--;
            return duration;
        }
        if (duration.Hours > 0 && duration.Minutes == 0)
        {
            duration.Hours--;
            duration.Minutes = 59;
            return duration;
        }
        throw new Exception("Duration cannot be negative");
    }

    public static bool operator <=(Duration duration1, Duration duration2)
    {
        int totalSeconds1 = duration1.TotalSeconds();
        int totalSeconds2 = duration2.TotalSeconds();

        return totalSeconds1 <= totalSeconds2;
    }

    public static bool operator >=(Duration duration1, Duration duration2)
    {
        int totalSeconds1 = duration1.TotalSeconds();
        int totalSeconds2 = duration2.TotalSeconds();

        return totalSeconds1 >= totalSeconds2;
    }
}
