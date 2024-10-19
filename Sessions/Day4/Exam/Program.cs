// See https://aka.ms/new-console-template for more information
using Exam;

Console.WriteLine("Hello, World!");


Dictionary<Question, List<Answer>> exam = new();
exam.Add(new Question("What is 2+2?", 1, new Answer(1, "4")),
         new List<Answer> { new Answer(1, "4") , new Answer(2, "5") , new Answer(3, "6") , new Answer(4, "7") });

exam.Add(new Question("What is 3*3?", 1, new Answer(3, "9")),
         new List<Answer> { new Answer(1, "4") , new Answer(2, "5") , new Answer(3, "9") , new Answer(4, "7") });

exam.Add(new Question("What is log2(16)?", 1, new Answer(4, "4")),
         new List<Answer> { new Answer(1, "4") , new Answer(2, "5") , new Answer(3, "6") , new Answer(4, "4") });

foreach (KeyValuePair<Question, List<Answer>> item in exam)
{
    Console.WriteLine(item.Key);
    foreach (Answer ans in item.Value)
    {
        Console.WriteLine(ans);
    }
    Console.WriteLine(new string('=', 20));
}