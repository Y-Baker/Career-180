Console.WriteLine("Menu-Driven Calculator");

Console.Write("Enter the first integer: ");
bool isNumber = int.TryParse(Console.ReadLine(), out int firstNumber);
while (!isNumber)
{
    Console.Write("Invalid input. Please enter a valid integer: ");
    isNumber = int.TryParse(Console.ReadLine(), out firstNumber);
}

Console.Write("Enter the second integer: ");
isNumber = int.TryParse(Console.ReadLine(), out int secondNumber);
while (!isNumber)
{
    Console.Write("Invalid input. Please enter a valid integer: ");
    isNumber = int.TryParse(Console.ReadLine(), out secondNumber);
}

Console.WriteLine("\nHere are the options:");
Console.WriteLine("1-Addition.");
Console.WriteLine("2-Subtraction.");
Console.WriteLine("3-Multiplication.");
Console.WriteLine("4-Division.");
Console.WriteLine("5-Exit.");

while (true)
{
    Console.Write("\nInput your choice: ");
    isNumber = int.TryParse(Console.ReadLine(), out int choice);
    while (!isNumber || choice < 1 || choice > 5)
    {
        Console.Write("Invalid input. Please enter a valid choice: ");
        isNumber = int.TryParse(Console.ReadLine(), out choice);
    }

    switch (choice)
    {
        case 1:
            Console.WriteLine($"{firstNumber} + {secondNumber} = {firstNumber + secondNumber}");
            break;
        case 2:
            Console.WriteLine($"{firstNumber} - {secondNumber} = {firstNumber - secondNumber}");
            break;
        case 3:
            Console.WriteLine($"{firstNumber} * {secondNumber} = {firstNumber * secondNumber}");
            break;
        case 4:
            if (secondNumber == 0)
            {
                Console.WriteLine("Cannot divide by zero.");
            }
            else
            {
                Console.WriteLine($"{firstNumber} / {secondNumber} = {firstNumber / (double)secondNumber}");
            }
            break;
        case 5:
            Console.WriteLine("Exiting the program.");
            break;
    }

    if (choice == 5)
    {
        break;
    }
}
