using System;

namespace Company
{
    class Program
    {
        static void Main(string[] args)
        {
            // Employee[] employees = ReadEmployees();

            Employee[] employees = new Employee[] {
                new Employee("employee 1", 1000, new _Date(1, 10, 2024), Gender.Male),
                new Employee("employee 2", 2000, new _Date(1, 10, 2023), Gender.Female),
                new Employee("employee 3", 3000, new _Date(1, 1, 2025), Gender.Male),
                new Employee("employee 4", 4000, new _Date(15, 9, 2023), Gender.Female)
            };

            Employee.SortEmployees(ref employees);

            foreach (Employee employee in employees)
            {
                Console.WriteLine(employee);
            }

        }
        
        static Employee ReadEmployee()
        {
            Console.WriteLine("Enter Employee Name: ");
            string name = Console.ReadLine() ?? "";
            Console.WriteLine("Enter Employee Salary: ");
            float salary = float.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter Employee Hire Date (dd-mm-yyyy): ");
            string[] date = Console.ReadLine()!.Split('-');
            _Date hireDate = new _Date(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]));
            Console.WriteLine("Enter Employee Gender (M/F): ");
            string option = Console.ReadLine()!.ToUpper();
            Gender gender = option switch
            {
                "M" => Gender.Male,
                "F" => Gender.Female,
                _ => throw new Exception("Invalid Gender")
            };

            return new Employee(name, salary, hireDate, gender);
        }

        static Employee[] ReadEmployees()
        {
            Console.WriteLine("Enter number of Employees: ");
            int count = int.Parse(Console.ReadLine()!);

            Employee[] employees = new Employee[count];

            for (int i = 0; i < count; i++)
            {
                employees[i] = ReadEmployee();
            }

            return employees;
        }
    }
}
