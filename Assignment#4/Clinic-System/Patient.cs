namespace ClinicSystem;

public class Patient
{
    public string Name { get; set; }
    public string Number { get; set; }
    public string Address { get; set; }
    public int Age { get; set; }
    public Gendre Gendre { get; set; }
    public List<Appoiment> History { get; set; }

    public Patient(string name, string number, string address, int age, Gendre gendre)
    {
        Name = name;
        Number = number;
        Address = address;
        Age = age;
        Gendre = gendre;
        History = new();
    }
}
