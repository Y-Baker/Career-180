using System.ComponentModel.DataAnnotations;

namespace StudentService;

public class Student
{
    public Guid Id { get; init; }
    public required string Name { get; set; }
    string? email;
    public required string Email { 
        get
        {
            return email ?? "";
        }
        set
        {
            if (new EmailAddressAttribute().IsValid(email))
                email = value;
            else
                throw new Exception("Not Valid Email");
        }}
    public string? Mobile { get; set; }
    public int Age { get; set; }
    public int TotalCreditHours { get; set; }
    float gpa;
    public float GPA
    {
        get => gpa;
        set
        {
            if (value < 0 || value > 4)
                throw new Exception("GPA must be between 0 and 4");
            gpa = value;
        }
    }

    public Student()
    {
        Id = Guid.NewGuid();
    }
    public Student(string name, string email, string mobile, int age, int creditHours, float gpa)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Mobile = mobile;
        Age = age;
        TotalCreditHours = creditHours;
        GPA = gpa;
    }

    public void AddScore(int courseHours, float grade)
    {
        if (grade < 0 || grade > 4)
            throw new Exception("Grade must be between 0 and 4");
        float points = GPA * TotalCreditHours;
        float newPoints = grade * courseHours;

        TotalCreditHours += courseHours;
        GPA = (points + newPoints) / TotalCreditHours;
    }

    public string View()
    {
        return $"{Id} {Name} {Email} {Mobile} {Age} {TotalCreditHours} {GPA}";
    }
}
