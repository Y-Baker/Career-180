using System;
using System.Text;

namespace Task
{
    class Program
    {
        const int numberOfStudent = 6;
        const int numberOfSubject = 4;
        static int[,]? readInput()
        {
            int[,] scores = new int[numberOfStudent, 1 + numberOfSubject];
            for (int i = 0; i < numberOfStudent; i++)
            {
                try
                {
                    string[] input = Console.ReadLine()!.Split(' ');
                    scores[i, 0] = int.Parse(input[0]);
                    for (int j = 1; j <= numberOfSubject; j++)
                        scores[i, j] = int.Parse(input[j]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return null;
                }
            }
            return scores;
        }

        static char getGrade(int score)
        {
            if (score >= 85)
                return 'A';
            if (score >= 70)
                return 'B';
            if (score >= 65)
                return 'C';
            if (score >= 50)
                return 'D';
            return 'F';
        }

        static char getGrade(int[] scores)
        {
            int sum = 0;
            for (int i = 0; i <= numberOfSubject; i++)
                sum += scores[i];
            return getGrade(sum / numberOfSubject);
        }

        static void printFailedStudents(List<int> failedStudents, int[,] scores)
        {
            Console.WriteLine("Failed Students:");
            foreach (int student in failedStudents)
                Console.WriteLine($"StudentId: {student}");
        }

        static void Main(string[] args)
        {
            int[,]? scores = readInput() ?? null;
            if (scores == null)
                return;
            List<int> failedStudents = new();
            Console.WriteLine("StudentId\tSubject1  Subject2  Subject3  Subject4  TotalGrade");
            for (int i = 0; i < numberOfStudent; i++)
            {
                int[] studentScores = new int[1 + numberOfSubject];
                int numberFailed = 0;
                Console.Write($"{scores[i, 0]} \t\t");
                for (int j = 1; j <= numberOfSubject; j++)
                {
                    char subjectGrade = getGrade(scores[i, j]);
                    Console.Write($"{subjectGrade} \t  ");
                    studentScores[j] = scores[i, j];
                    if (subjectGrade == 'F')
                        numberFailed++;
                }
                Console.WriteLine(getGrade(studentScores));
                if (numberFailed > 2)
                    failedStudents.Add(scores[i, 0]);
            }
            Console.WriteLine();
            printFailedStudents(failedStudents, scores);
        }
    }
}