namespace Delegate_Event;
public enum State
{
    FirstTime,
    Returning
}

public class Employee
{
    public static event Action<Employee, State> AddEmployee;

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public double Salary { get; set; }
    private static int _count = 0;

    public Employee(string name, string email, int age, double salary)
    {
        Id = ++_count;
        Name = name;
        Salary = salary;
        Email = email;
        Age = age;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Email: {Email}, Age: {Age}, Salary: {Salary:C}";
    }

    public void OnAddEmployee()
    {
        AddEmployee.Invoke(this, State.FirstTime);
    }
}
