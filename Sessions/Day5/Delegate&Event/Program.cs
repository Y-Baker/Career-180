// See https://aka.ms/new-console-template for more information
using Delegate_Event;

Console.WriteLine("Hello, World!");

Employee employee1 = new("Yousef", "yuossefbakier@gmail.com", 20, 3000);

Club club = new("Football", "Egypt", "Cairo");

SocialEnsurance socialEnsurance = new("IT", 1200);

Employee.AddEmployee += club.AddInClub;
Employee.AddEmployee += socialEnsurance.BeginSocialEnsurance;

employee1.OnAddEmployee();