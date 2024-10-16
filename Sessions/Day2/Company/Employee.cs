namespace Company;

public class Employee
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public float Salary { get; set; }
    public _Date hireDate { get; set; }
    public Gender Gender { get; set; }

    public Employee(string name, float salary, _Date hire, Gender gender)
    {
        Id = Guid.NewGuid();
        Name = name;
        Salary = salary;
        hireDate = hire;
        Gender = gender;
    }

    public override string ToString()
    {
        return $"Name: {Name}, Salary: {Salary:C}, Hire Date: {hireDate}, Gender: {Gender}";
    }

    public static void SortEmployees(ref Employee[] employees)
    {
        Array.Sort(employees,
            (e1, e2) => {
                if (e1.hireDate >= e2.hireDate)
                    return 1;
                else
                    return -1;
            });
    }
}
