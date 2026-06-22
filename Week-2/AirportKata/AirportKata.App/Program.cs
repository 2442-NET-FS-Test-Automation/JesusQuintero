using System.Runtime.CompilerServices;
using Airport.Domain;
using Serilog;
//using Airport.Domain;

namespace Airport.App;


public class Program
{

    //public static List<CommercialAirplane> myPlanes = new List<CommercialAirplane>();
    public static Catalog myPlanes = new();
    private static CommercialAirplane? lastRegisteredAirplane;



    public static async Task Main()
    {
        bool isRunning = true;
        string? option;

        myPlanes.Add(new CommercialAirplane("Boeing 787", 30, "JET", 2, "AeroMexico"));
        myPlanes.Add(new CommercialAirplane("Airbus A320neo", 20, "JET", 2, "VivaAerobus"));
        myPlanes.Add(new CommercialAirplane("Airbus A220", 15, "JET" , 2, "VivaAerobus"));

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
            Console.WriteLine("8.- Undo Register Airplane");
            Console.WriteLine("9.- Serve Next Airplane");
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
                case "8":
                    UndoLastAction();
                    break;
                case "9":
                    ServeNextAirplane();
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
        int capacity = 0;
        int firstCapacity;
        int age;
        string? engineModel = null;
        int engineCount;
        // int maxSpeed = 0;
        // int maxAltitude = 0;


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

        while(engineModel is null)
        {
            Console.Write("Engine Model: ");
            engineModel = Console.ReadLine();
            if(engineModel is null) Console.WriteLine("Please insert the engine model\n");
        }
        engineCount = GetIntegerInput("Number of engines: ", "number", 5);

        age = GetIntegerInput("Age: ", "years");
        capacity = GetIntegerInput("Capacity: ", "capacity",capacity);
        firstCapacity = GetIntegerInput("First class capacity: ", "First Class capacity",capacity);

        // Modificar cuando se tengan diccionarios
        CommercialAirplane newplane = new CommercialAirplane(model, 
                                                             age, 
                                                             engineModel, 
                                                             engineCount, 
                                                             airline, 
                                                             firstCapacity, 
                                                             capacity);

        myPlanes.Add(newplane);
        lastRegisteredAirplane = newplane;

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

        if (!finded)
        {
            Log.Warning("Lookout failed for Id: {inputID}",inputID);
            throw new AirplaneNotFoundException(inputID);
        } 

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
        Airplanes[]? foundPlanes = await Task.WhenAll(fetchedAirplanes);

        Airplanes? firstPlaneFound = foundPlanes.Length > 0 ? foundPlanes[0] : null;
        if(firstPlaneFound != null)
        {
            myPlanes.Add(firstPlaneFound);
            Log.Information("New plane added");
        }
        else
        {
            Log.Warning("No plane founded via API");
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
            Log.Information("Insert a valid Id");
            idInput = Console.ReadLine();

            if (!int.TryParse(idInput, out idPrio)) Console.WriteLine("Please insert a valid number\n");
            else if (idPrio <= 0 || idPrio > myPlanes.Count) Console.WriteLine("Please insert a valid Id\n");
        }
        var reordered = myPlanes.Planes
            .OrderByDescending(p => p.Id == idPrio)
            .ToList();

        myPlanes.SetList(reordered);
        Console.WriteLine();

    }

    public static void UndoLastAction()
    {
        Console.Clear();
        Console.WriteLine("========= Undo Register Airplane =========\n");

        if (lastRegisteredAirplane is null || !myPlanes.Contains(lastRegisteredAirplane))
        {
            Log.Information("No registered airplane to undo");
            Console.ReadLine();
            return;
        }

        myPlanes.Remove(lastRegisteredAirplane);
        Console.WriteLine($"Undo complete: airplane {lastRegisteredAirplane.Id} removed");
        lastRegisteredAirplane = null;
        Console.ReadLine();
    }

    public static void ServeNextAirplane()
    {
        Console.Clear();
        Console.WriteLine("========= Serve Next Airplane =========\n");

        if (myPlanes.Count == 0)
        {
            // Console.WriteLine("No airplanes pending to be served");
            Log.Information("No airplanes pending to be served");
            Console.ReadLine();
            return;
        }

        Airplanes nextAirplane = myPlanes.Planes[0];

        Console.WriteLine("Next airplane served:");
        nextAirplane.GetInfo();
        myPlanes.Remove(nextAirplane);
        Console.WriteLine($"\nRemaining airplanes: {myPlanes.Count}");
        Console.ReadLine();
    }
}
