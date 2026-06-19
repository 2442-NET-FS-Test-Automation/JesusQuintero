using System.Runtime.CompilerServices;
using Airport.Domain;
using Serilog;
//using Airport.Domain;

namespace Airport.App;


public class Program
{

    public static List<CommercialAirplane> myPlanes = new List<CommercialAirplane>();



    public static async Task Main()
    {
        bool isRunning = true;
        string? option;

        myPlanes.Add(new CommercialAirplane("Boeing 787", 300, 3000, 800, 30, "AeroMexico"));
        myPlanes.Add(new CommercialAirplane("Airbus A320neo", 200, 2800, 700, 15, "VivaAerobus"));
        myPlanes.Add(new CommercialAirplane("Airbus A220", 150, 2800, 800, 10, "VivaAerobus"));

        while (isRunning)
        {

            Console.WriteLine("========= Airplane Manage System =========\n");
            Console.WriteLine("1.- Register Airplane");
            Console.WriteLine("2.- Charge Airplane");
            Console.WriteLine("3.- Retire Airplane");
            Console.WriteLine("4.- List of Airplanes");
            Console.WriteLine("5.- Boarded Airplanes");
            Console.WriteLine("6.- Test API");
            Console.WriteLine("7.- Priority an Airplane");
            Console.WriteLine("X.- Leave");


            option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Program.CreateCommercialAirplane();
                    break;
                case "2":
                    Program.BoardAirplane();
                    break;
                case "3":
                    Program.RetireAirplane();
                    break;
                case "4":
                    Program.ListAirplanes();
                    break;
                case "5":
                    GetBoardedAirplanes();
                    break;
                case "6":
                    await AssyncHttpRequest();
                    break;
                case "7":
                    PutAtFirst();
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

        while (model is null)
        {
            Console.Write("Airplane model: ");
            model = Console.ReadLine();
            if (model is null) Console.WriteLine("Please insert a model of an airplane\n");
        }

        while (airline is null)
        {
            Console.Write("Airline: ");
            airline = Console.ReadLine();
            if (airline is null) Console.WriteLine("Please insert a name of an airline\n");
        }

        while (capacity_s is null || capacity <= 0)
        {
            Console.Write("Capacity: ");
            capacity_s = Console.ReadLine();
            if (!int.TryParse(capacity_s, out capacity)) Console.WriteLine("Please insert a valid number\n");
            else if (capacity <= 0) Console.WriteLine("Please insert a positive number\n");
        }

        capacity_s = null;

        while (capacity_s is null || firstCapacity <= 0 || firstCapacity > capacity)
        {
            Console.Write("First Class Capacity: ");
            capacity_s = Console.ReadLine();
            if (!int.TryParse(capacity_s, out firstCapacity)) Console.WriteLine("Please insert a valid number\n");
            else if (firstCapacity <= 0) Console.WriteLine("Please insert a positive number\n");
            else if (firstCapacity > capacity) Console.WriteLine("First Class exeeds plane capacity\n");
        }

        capacity_s = null;

        while (capacity_s is null || maxSpeed <= 0)
        {
            Console.Write("Max Speed (Km/h): ");
            capacity_s = Console.ReadLine();
            if (!int.TryParse(capacity_s, out maxSpeed)) Console.WriteLine("Please insert a valid number\n");
            else if (maxSpeed <= 0) Console.WriteLine("Please insert a positive number\n");
        }

        capacity_s = null;

        while (capacity_s is null || maxAltitude <= 0)
        {
            Console.Write("Max Altitude (m): ");
            capacity_s = Console.ReadLine();
            if (!int.TryParse(capacity_s, out maxAltitude)) Console.WriteLine("Please insert a valid number\n");
            else if (maxAltitude <= 0) Console.WriteLine("Please insert a positive number\n");
        }

        CommercialAirplane newplane = new CommercialAirplane(model, capacity, maxAltitude, maxSpeed, firstCapacity, airline);

        myPlanes.Add(newplane);

        Console.WriteLine("\nNew airplane added");
        return;

    }

    // Get the information to board an airplane
    public static void BoardAirplane()
    {
        Console.Clear();
        int inputID;
        bool finded = false;
        int[] passengers = new int[2];

        Console.WriteLine("========= Board an Airplane =========\n\n");
        if (myPlanes.Count <= 0)
        {
            Console.WriteLine("Any airplanes registered");
            Console.ReadLine();
            return;
        }
        bool uncharged = false;


        foreach (CommercialAirplane plane in myPlanes)
        {
            if (!plane.Boarded)
            {
                uncharged = true;
                plane.GetInfo();
            }
        }

        if (!uncharged)
        {
            Console.WriteLine("All airplanes are boarded");
            Console.ReadLine();
            return;
        }

        inputID = GetIntegerInput("Please type the Id to board: ", "Id");

        foreach (CommercialAirplane plane in myPlanes)
        {
            if (plane.Id == inputID)
            {
                passengers[0] = GetIntegerInput("Number of commercial class boarding: ",
                                                "Commercial Class",
                                                limit: plane.Capacity - plane.FirstClassCapacity);
                passengers[1] = GetIntegerInput("Number of first class boarding: ",
                                                "First Class",
                                                limit: plane.FirstClassCapacity);
                plane.Board(passengers);
                Console.ReadLine();
                return;
            }
        }

        if (!finded) Console.WriteLine($"Lookout failed for Id: {inputID}");

        Console.ReadLine();
    }


    // Removes an airplane from our list of airplanes
    public static void RetireAirplane()
    {
        Console.Clear();
        int inputID;
        bool finded = false;

        Console.WriteLine("========= Retire an Airplane =========\n\n");
        if (myPlanes.Count <= 0)
        {
            Console.WriteLine("No airplanes to retire");
            Console.ReadLine();
            return;
        }


        foreach (CommercialAirplane plane in myPlanes)
        {
            plane.GetInfo();
        }

        inputID = GetIntegerInput("\nPlease type the Id to retire: ", "ID");

        foreach (CommercialAirplane plane in myPlanes)
        {
            if (plane.Id == inputID)
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

    // Print all the airplanes status
    public static void ListAirplanes()
    {
        Console.Clear();
        Console.WriteLine("========= My Airplanes =========\n\n");
        if (Program.myPlanes.Count == 0)
        {
            Console.WriteLine("We don't have any airplanes");
            Console.ReadLine();
            return;
        }

        var properties = typeof(CommercialAirplane).GetProperties();

        foreach (var property in properties)
        {
            Console.Write($"{property.Name,-20}");
        }
        Console.WriteLine();

        foreach (CommercialAirplane plane in myPlanes)
        {
            foreach (var property in properties)
            {
                var value = property.GetValue(plane);
                Console.Write($"{value,-20}");
            }
            Console.WriteLine();
        }

        Console.ReadLine();
    }

    // Print the boarded airplanes status
    public static void GetBoardedAirplanes()
    {
        Console.Clear();
        Console.WriteLine("========= Boarded Airplanes =========\n\n");
        foreach (CommercialAirplane plane in myPlanes)
        {
            if (!plane.Boarded) continue;
            plane.BoardStatus();
        }
        Console.ReadLine();
    }


    // Get and validate integer input from the user
    public static int GetIntegerInput(string message, string dataName = "number", int limit = -1)
    {
        string? input = null;
        int inputID;

        while (input != null)
        {
            Console.Write(message);
            input = Console.ReadLine();
            if (!int.TryParse(input, out inputID)) Console.WriteLine($"Please insert a valid {dataName}\n");
            else if (inputID <= 0) Console.WriteLine($"Please insert a positive {dataName}\n");
            else if (limit != -1 && inputID > limit) Console.WriteLine($"The given number exceeds the limit of {dataName}");
            else return inputID;

            input = null;
        }

        return -1;
    }

    public static async Task AssyncHttpRequest()
    {
        OpenAircraftClient client = new();

        string[] isbn = { "8b90a9a6019b99352399dd56f8664ef0" };

        Task<Airplanes?>[] fetchedAirplanes = new Task<Airplanes?>[isbn.Length];

        for (int i = 0; i < isbn.Length; i++)
        {
            fetchedAirplanes[i] = client.FetchByIdAsync(isbn[i]);
        }
    }

    public static void PutAtFirst()
    {
        Console.WriteLine("========= Priority in a Plane =========\n\n");
        Console.WriteLine("Insert plane Id to make it priority: ");
        string? idInput = Console.ReadLine();
        int idPrio = 0;

        while (idInput is null || idPrio <= 0)
        {
            Console.WriteLine("Insert a valid Id");
            idInput = Console.ReadLine();

            if (!int.TryParse(idInput, out idPrio)) Console.WriteLine("Please insert a valid number\n");
            else if (idPrio <= 0 || idPrio > myPlanes.Count) Console.WriteLine("Please insert a valid Id\n");
        }
        var reordered = myPlanes
            .OrderByDescending(p => p.Id == idPrio)
            .ToList();

        myPlanes = reordered;
        Console.WriteLine();

    }
}