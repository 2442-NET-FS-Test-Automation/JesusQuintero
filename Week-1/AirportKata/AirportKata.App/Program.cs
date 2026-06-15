using System.Runtime.CompilerServices;
using Airport.Domain;

namespace Airport.App;


public class Program
{
    public static void Main()
    {
        bool isRunning = true;
        string option;

        while (isRunning)
        {
            Console.WriteLine("========= Airport Manage System =========\n");
            Console.WriteLine("1.- Next Flights");
            Console.WriteLine("2.- Airlines");
            Console.WriteLine("3.- Destinations");
            Console.WriteLine("X.- Leave");


            option = Console.ReadLine();

            switch (option)
            {
                case "1":

                    break;
                case "2":

                    break;
                case "3":

                    break;
                case "4":
                    isRunning = false;
                    break;
                default:

                break;

            }

            
            Console.Clear();
        }
    }
}