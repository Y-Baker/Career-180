// See https://aka.ms/new-console-template for more information
using DSA;

Console.WriteLine("Hello, World!");

_Queue<int> q = new(5);
Console.WriteLine(q.Pop());
q.Push(1);
q.Push(2);
q.Push(3);
q.Push(4);
q.Push(5);

Console.WriteLine(q.Pop());
Console.WriteLine(q.Pop());
Console.WriteLine(q.Pop());
Console.WriteLine(q.Pop());
Console.WriteLine(q.Pop());
Console.WriteLine(q.Pop());

