namespace ConsoleApp;

internal class Student
{
    public int ID { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public List<Subject> Subjects { get; set; }

    public Student() {
        Subjects = new List<Subject>();
    }
    public Student(int id, string firstName, string lastName, List<Subject> subjects)
    {
        ID = id;
        FirstName = firstName;
        LastName = lastName;
        Subjects = subjects;
    }

    public override string ToString()
    {
        string subjects = string.Empty;
        foreach (var subject in Subjects)
            subjects += subject.ToString() + ", ";

        return $"{ID} - {FirstName} {LastName} - Subjects -> {subjects}";
    }

}
