using System;

namespace Delegate_Event;

public class Club
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public DateTime FoundedTime { get; set; }

    private static int _count = 0;

    public Club(string name, string country, string city)
    {
        Id = ++_count;
        Name = name;
        Country = country;
        City = city;
        FoundedTime = DateTime.Now;
    }

    public override string ToString()
    {
        return $"Club: {Name}, Country: {Country}, City: {City}, Founded: {FoundedTime}";
    }

    public void AddInClub(Employee employee, State state)
    {
        Console.WriteLine(employee);
        Console.WriteLine($"Employee {employee.Name} is added in {Name} club as {state}");
    }
}
