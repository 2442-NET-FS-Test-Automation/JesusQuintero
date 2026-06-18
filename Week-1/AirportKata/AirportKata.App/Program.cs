using System.Runtime.CompilerServices;
using Airport.Domain;
//using Airport.Domain;

namespace Airport.App;


public class Program
{

    public static List<CommercialAirplane> myPlanes = new List<CommercialAirplane>();

    
    public static void Main()
    {
        bool isRunning = true;
        string? option;



        while (isRunning)
        {
            
            Console.WriteLine("========= Airplane Manage System =========\n");
            Console.WriteLine("1.- Register Airplane");
            Console.WriteLine("2.- Charge Airplane");
            Console.WriteLine("3.- Retire Airplane");
            Console.WriteLine("4.- List of Airplanes");
            Console.WriteLine("X.- Leave");


            option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Program.CreateCommercialAirplane();
                    break;
                case "2":
                    Program.ChargeAirplane();
                    break;
                case "3":
                    Program.RetireAirplane();
                    break;
                case "4":
                    Program.ListAirplanes();
                    break;
                case "x":
                    isRunning = false;
                    break;
                case "X":
                    isRunning = false;
                    break;
                default:

                break;

            }

            Console.ReadLine();
            Console.Clear();
        }
    }

    // Get the information to create a CommercialAriplane
    public static void CreateCommercialAirplane()
    {
        Console.Clear();

        string? model = null;
        string? airline = null;
        string? capacity_s = null; // This string will be used to get strings for integer inputs and convert it later
        int capacity = 0;
        int firstCapacity = 0;
        int maxSpeed = 0;
        int maxAltitude = 0;


        Console.WriteLine("========== CREATE COMMERCIAL AIRPLANE ==========");
        
        while(model is null)
        {
            Console.Write("Airplane model: ");
            model = Console.ReadLine();
            if(model is null) Console.WriteLine("Please insert a model of an airplane\n");
        }

        while(airline is null)
        {
            Console.Write("Airline: ");
            airline = Console.ReadLine();
            if(airline is null) Console.WriteLine("Please insert a name of an airline\n");
        }

        while(capacity_s is null || capacity <= 0)
        {
            Console.Write("Capacity: ");
            capacity_s = Console.ReadLine();
            if(! int.TryParse(capacity_s, out capacity)) Console.WriteLine("Please insert a valid number\n");
            else if(capacity <= 0) Console.WriteLine("Please insert a positive number\n");
        }

        capacity_s = null;

        while(capacity_s is null || firstCapacity <= 0 || firstCapacity > capacity)
        {
            Console.Write("First Class Capacity: ");
            capacity_s = Console.ReadLine();
            if(! int.TryParse(capacity_s, out firstCapacity)) Console.WriteLine("Please insert a valid number\n");
            else if(firstCapacity <= 0) Console.WriteLine("Please insert a positive number\n");
            else if(firstCapacity > capacity) Console.WriteLine("First Class exeeds plane capacity\n");
        }

        capacity_s = null;

        while(capacity_s is null || maxSpeed <= 0)
        {
            Console.Write("Max Speed (Km/h): ");
            capacity_s = Console.ReadLine();
            if(! int.TryParse(capacity_s, out maxSpeed)) Console.WriteLine("Please insert a valid number\n");
            else if(maxSpeed <= 0) Console.WriteLine("Please insert a positive number\n");
        }

        capacity_s = null;

        while(capacity_s is null || maxAltitude <= 0)
        {
            Console.Write("Max Altitude (m): ");
            capacity_s = Console.ReadLine();
            if(! int.TryParse(capacity_s, out maxAltitude)) Console.WriteLine("Please insert a valid number\n");
            else if(maxAltitude <= 0) Console.WriteLine("Please insert a positive number\n");
        }

        CommercialAirplane newplane = new CommercialAirplane(model, capacity, maxAltitude, maxSpeed, firstCapacity, airline);

        myPlanes.Add(newplane);

        Console.WriteLine("\nNew airplane added");
        return;
        
    }

    public static void ChargeAirplane()
    {
        Console.Clear();
        int inputID;
        bool finded = false;

        Console.WriteLine("========= Charge an Airplane =========\n\n");
        if(myPlanes.Count <= 0)
        {
                Console.WriteLine("Any airplanes registered");
                Console.ReadLine();
                return;
        }
        bool uncharged = false;


        foreach(CommercialAirplane plane in myPlanes)
        {
            if (!plane.Status)
            {
                uncharged = true;
                plane.GetInfo();
            }
        }

        if (!uncharged)
        {
            Console.WriteLine("All airplanes are charged");
            Console.ReadLine();
            return;
        }

        inputID = GetIntegerInput("Please type the Id to charge: ", "Id");
        
        foreach(CommercialAirplane plane in myPlanes)
        {
            if(plane.Id == inputID)
            {
                plane.ChargeAirplane();
                Console.ReadLine();
                return;
            }
        }

        if (!finded) Console.WriteLine($"Lookout failed for Id: {inputID}");
        
        Console.ReadLine();
    }


    public static void RetireAirplane()
    {
        Console.Clear();
        int inputID;
        bool finded = false;

        Console.WriteLine("========= Retire an Airplane =========\n\n");
        if(myPlanes.Count <= 0)
        {
                Console.WriteLine("No airplanes to retire");
                Console.ReadLine();
                return;
        }


        foreach(CommercialAirplane plane in myPlanes)
        {
            plane.GetInfo();
        }

        inputID = GetIntegerInput("\nPlease type the Id to retire: ", "ID");
            
        foreach(CommercialAirplane plane in myPlanes)
        {
            if(plane.Id == inputID)
            {
                myPlanes.Remove(plane);
                finded = true;
                Console.WriteLine("Plane retired");
                Console.ReadLine();
                return;
            }
        }

        if (!finded)
        {
            Console.WriteLine($"Lookout failed for Id: {inputID}");
        }
        

        Console.ReadLine();
    }

    public static void ListAirplanes()
    {
        Console.WriteLine("========= My Airplanes =========\n\n");
        if(Program.myPlanes.Count == 0)
        {
            Console.WriteLine("We don't have any airplanes");
            Console.ReadLine();
            return;
        }

        foreach(CommercialAirplane plane in Program.myPlanes)
        {
            Console.WriteLine(plane);
        }

        Console.ReadLine();
    }

    public static int GetIntegerInput(string message, string dataName = "number")
    {
        string? input = null;
        int inputID;

        while(input != null)
        {
            Console.Write(message);
            input = Console.ReadLine();
            if(! int.TryParse(input, out inputID)) Console.WriteLine($"Please insert a valid {dataName}\n");
            else if(inputID <= 0) Console.WriteLine($"Please insert a positive {dataName}\n");
            else return inputID;

            input = null;
        }

        return -1;
    }
}