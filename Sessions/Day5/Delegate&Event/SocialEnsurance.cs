using System;

namespace Delegate_Event;

public class SocialEnsurance
{
    public int Id { get; set; }
    public string Department { get; set; }
    public double MinSalary { get; set; }
    public int numberOfEmployees { get; set; }

    private static int _count = 0;

    public SocialEnsurance(string department, double minSalary)
    {
        Id = ++_count;
        Department = department;
        MinSalary = minSalary;
        numberOfEmployees = 0;
    }

    public override string ToString()
    {
        return $"Social Ensurance of {Department} department, Minimum Salary: {MinSalary : C}, Number of Employees: {numberOfEmployees}";
    }

    public void BeginSocialEnsurance(Employee employee, State state)
    {
        Console.WriteLine(employee);
        if (employee.Salary < MinSalary)
            Console.WriteLine($"Employee {employee.Name} is not eligible for this Social Ensurance");
        else
        {
            Console.WriteLine($"Employee {employee.Name} is added in {Department} Social Ensurance as {state}");
            numberOfEmployees++;
        }
    }
}
