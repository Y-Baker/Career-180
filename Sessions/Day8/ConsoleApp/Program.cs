using ConsoleApp;

List<int> numbers = new List<int>() { 2,4,6,7,1,4,2,9,1};

#region Query 1
var q1 = numbers.Distinct().Order();
//foreach (int x in q1)
//    Console.WriteLine(x);
#endregion

#region Query 2
var q2 = q1.Select(x => new { Number = x, Multiply = x * x });
//foreach (var x in q2)
//    Console.WriteLine(x);
#endregion


string[] names = { "Tom", "Dick", "Harry", "MARY", "Jay" };

#region Query 1
var q3 = names.Where(s => s.Length == 3);
//foreach (string s in q3)
//    Console.WriteLine(s);
#endregion

#region Query 2
var q4 = names.Where(s => s.ToLower().Contains('a')).OrderBy(s => s.Length);
//foreach (string s in q4)
//    Console.WriteLine(s);
#endregion

#region Query 3
var q5 = names.Take(2);
//foreach (string s in q5)
//    Console.WriteLine(s);
#endregion

List<Student> students = new List<Student>()
{
    new Student()
    {
        ID = 1,
        FirstName = "Ali",
        LastName = "Mohammed",
        Subjects = new List<Subject>()
        {
            new Subject() { Code = 22, Name = "EF" },
            new Subject() { Code = 33, Name = "UML" }
        }
    },
    new Student()
    {
        ID = 2,
        FirstName = "Mona",
        LastName = "Gala",
        Subjects = new List<Subject>()
        {
            new Subject() { Code = 22, Name = "EF" },
            new Subject() { Code = 34, Name = "XML" },
            new Subject() { Code = 25, Name = "JS" }
        }
    },
    new Student()
    {
        ID = 3,
        FirstName="Yara",
        LastName="Yousf",
        Subjects = new List<Subject>()
        {
            new Subject (){ Code=22,Name="EF"},
            new Subject (){ Code=25,Name="JS"}
        }
    },
    new Student()
    {
        ID = 1,
        FirstName="Ali",
        LastName="Ali",
        Subjects = new List<Subject>()
        {
            new Subject (){ Code=33,Name="UML"
        }
    }
}
};

#region Query 1
var q6 = students.Select(s => new { FullName = $"{s.FirstName} {s.LastName}", NoOfSubjects = s.Subjects.Count });
//foreach (var s in q6)
//    Console.WriteLine(s);
#endregion

#region Query 2
var q7 = students.OrderByDescending(s => s.FirstName).ThenBy(s => s.LastName).Select(s => $"{s.FirstName} {s.LastName}");
foreach (string s in q7)
    Console.WriteLine(s);
#endregion