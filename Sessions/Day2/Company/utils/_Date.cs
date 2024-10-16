using System.Diagnostics.CodeAnalysis;

public struct _Date
{
    public int Day { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }

    public _Date(int day, int month, int year)
    {
        Day = day;
        Month = month;
        Year = year;
    }

    public override string ToString()
    {
        return $"{Day:00}/{Month:00}/{Year}";
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is _Date date)
        {
            return date.Day == Day && date.Month == Month && date.Year == Year;
        }
        return false;
    }

    public static bool operator >(_Date date1, _Date date2)
    {
        if (date1.Year > date2.Year)
            return true;
        if (date1.Year < date2.Year)
            return false;

        if (date1.Month > date2.Month)
            return true;
        if (date1.Month < date2.Month)
            return false;

        return date1.Day > date2.Day;
    }

    public static bool operator <(_Date date1, _Date date2)
    {
        if (date1.Equals(date2))
            return false;
        return !(date1 > date2);
    }

    public static bool operator >=(_Date date1, _Date date2)
    {
        return date1 > date2 || date1.Equals(date2);
    }

    public static bool operator <=(_Date date1, _Date date2)
    {
        return date1 < date2 || date1.Equals(date2);
    }
}
