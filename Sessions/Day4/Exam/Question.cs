using System;

namespace Exam;

public class Question
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int Marks { get; set; }
    public Answer? CorrectAnswer { get; set; }
    private static int _count = 1;

    public Question(string body, int marks, Answer? correctAnswer = null)
    {
        Id = _count++;
        Body = body;
        Marks = marks;
        CorrectAnswer = correctAnswer;
    }

    public override string ToString()
    {
        return $"({Id}) {Body} - {Marks} marks - Correct Answer: {CorrectAnswer?.ToString() ?? "Not set"}";
    }
}
