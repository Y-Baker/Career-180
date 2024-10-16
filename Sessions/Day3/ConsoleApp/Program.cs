using TimeSpan;

Duration D1 = new Duration(1, 10, 15);
Console.WriteLine(D1);


Duration D2 = new Duration(7800);
Console.WriteLine(D2);

Duration D3 = new Duration(2, 10, 0);

if (!D1.Equals(D2))
{
    Console.WriteLine("D1 and D2 are not equal");
}

if (D2.Equals(D3))
{
    Console.WriteLine("D2 and D3 are equal");
}

D3 = D1 + D2;
Console.WriteLine(D3);

D3 = 666 + D3;
Console.WriteLine(D3);

D3 = D1++;
Console.WriteLine(D3);

D3 = --D2;
Console.WriteLine(D3);
Console.WriteLine(D2);

if (D1 <= D2)
{
    Console.WriteLine("D1 is less than or equal to D2");
}
else
{
    Console.WriteLine("D1 is greater than D2");
}