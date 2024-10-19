using System;

namespace Exam;

public struct Answer
{
    public int Num { get; set; }
    public string Body { get; set; }

    public Answer(int num, string body)
    {
        Num = num;
        Body = body;
    }

    public override string ToString()
    {
        return $"Num: {Num}, Body: {Body}";
    }
}
