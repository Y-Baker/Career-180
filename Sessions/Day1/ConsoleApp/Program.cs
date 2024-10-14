using StudentService;
using TimeSpan;

Console.WriteLine("Hello, World!");

Student student = new() { Name = "Yousef", Email = "yuossefbakier@gmail.com" };

student.AddScore(4, 3.7f);
student.AddScore(3, 3.5f);

Console.WriteLine(student.View());



Duration D1 = new Duration(1, 10, 15);
Console.WriteLine(D1.getstring());


Duration D2 = new Duration(7800);
Console.WriteLine(D2.getstring());
