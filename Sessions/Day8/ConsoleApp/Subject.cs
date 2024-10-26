namespace ConsoleApp;

internal class Subject
{
    public required int Code { get; set; }
    public required string Name { get; set; }

    public Subject() { }
    public Subject(int code, string name)
    {
        Code = code;
        Name = name;
    }

    public override string ToString() => $"{Code} - {Name}";
}
