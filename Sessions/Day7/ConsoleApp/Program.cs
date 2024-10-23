// See https://aka.ms/new-console-template for more information
using ConsoleApp;

Console.WriteLine("Hello, World!");
// #region Point3D
// const string path = "log.txt";
// // 3. Read from the User the Coordinates for 2 point P1, P2
// // (Check the input, tryPares ,  Parse , Convert )
// Point3D? p1 = null, p2 = null;
// do
// {
//     Console.WriteLine("Enter the coordinates for P1 (separated by a comma): ");
//     string input = Console.ReadLine()!;
//     string[] coordinates = input.Split(',');

//     try
//     {
//         if (coordinates.Length > 3)
//             throw new Exception("Invalid input. Please at most 3 coordinates.");
//         if (coordinates.Length < 1)
//             throw new Exception("Invalid input. Please at least 1 coordinates.");
//         bool xIsInt = int.TryParse(coordinates[0], out int x);
//         if (!xIsInt)
//             throw new Exception("Invalid input. Please enter a valid integer for X.");
//         int y = 0;
//         if (coordinates.Length >= 2)
//             y = int.Parse(coordinates[1]);
        
//         int z = 0;
//         if (coordinates.Length == 3)
//             z = Convert.ToInt32(coordinates[2]);
        
//         p1 = new Point3D(x, y, z);
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine(ex.Message);
//         using (StreamWriter sw = new(path, append: true))
//         {
//             sw.WriteLine($"{DateTime.Now} - {ex.Message}");
//             sw.WriteLine($"\t {ex.StackTrace}");
//             sw.WriteLine();
//         }
//         continue;
//     }


//     Console.WriteLine("Enter the coordinates for P2 (separated by a comma): ");
//     input = Console.ReadLine()!;
//     coordinates = input.Split(',');

//     try
//     {
//         if (coordinates.Length > 3)
//             throw new Exception("Invalid input. Please at most 3 coordinates.");
//         if (coordinates.Length < 1)
//             throw new Exception("Invalid input. Please at least 1 coordinates.");
//         bool xIsInt = int.TryParse(coordinates[0], out int x);
//         if (!xIsInt)
//             throw new Exception("Invalid input. Please enter a valid integer for X.");
//         int y = 0;
//         if (coordinates.Length >= 2)
//             y = int.Parse(coordinates[1]);
        
//         int z = 0;
//         if (coordinates.Length == 3)
//             z = Convert.ToInt32(coordinates[2]);
        
//         p2 = new Point3D(x, y, z);
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine(ex.Message);
//         using (StreamWriter sw = new(path, append: true))
//         {
//             sw.WriteLine($"{DateTime.Now} - {ex.Message}");
//             sw.WriteLine($"\t {ex.StackTrace}");
//             sw.WriteLine();
//         }
//         continue;
//     }
// } while (p1 is null || p2 is null);

// if (p1 == p2)
// {
//     Console.WriteLine("P1 and P2 are the same point.");
// } else
// {
//     Console.WriteLine("P1 and P2 are different points.");
//     Console.WriteLine($"P1: {p1}");
//     Console.WriteLine($"P2: {p2}");
// }

// #endregion

#region NIC
NIC nic = NIC.GetInstance();
Console.WriteLine(nic);

NIC nic2 = NIC.GetInstance();
Console.WriteLine(nic2);

if (nic == nic2)
{
    Console.WriteLine("The two NICs are the same.");
} else
{
    Console.WriteLine("The two NICs are different.");
}

#endregion