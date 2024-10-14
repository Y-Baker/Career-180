using StudentService;

Console.WriteLine("Hello, World!");

Student student = new() { Name = "Yousef", Email = "yuossefbakier@gmail.com" };

student.AddScore(4, 3.7f);
student.AddScore(3, 3.5f);

Console.WriteLine(student.View());
