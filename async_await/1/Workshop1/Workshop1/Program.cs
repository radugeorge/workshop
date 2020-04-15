using System;

namespace Workshop1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. Homework with thread");
            Console.WriteLine("2. Homework with task");
            Console.WriteLine("3. Homework with async await");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1": { HomeWork1.Run(); break; }
                case "2": { HomeWork2.Run(); break; }
                case "3": { HomeWork3.Run(); break; }
                default:
                    break;
            }
        }
    }
}
