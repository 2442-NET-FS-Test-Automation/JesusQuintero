using System.Runtime.CompilerServices;

namespace Airport.Domain;

public class CommercialAirplane : Airplanes, IBoardable
{
    public int FirstClassCapacity {get; set;} 
    public string Airline {get; set;}
    public bool Boarded = false;
    private int[] Passengers = {0, 0};

    public CommercialAirplane (string model, 
                               int airplaneAge, 
                               string engineModel, 
                               int engineCount,
                               string airline,
                               int firstClassCapacity = -1,
                               int capacity = -1) 
                                : base (model, airplaneAge, engineModel, engineCount, capacity)
    {
        FirstClassCapacity = firstClassCapacity;
        Airline = airline;

    }

    public override string ToString()
    {
        string status;
        if (Boarded == false) {
            status = "Unboard";
        }
        else
        {
            status = "Boarded";
        }
        return $"===== {Model} from {Airline}=====\nTurist Capacity: {Capacity - FirstClassCapacity}\nFirst Class: {FirstClassCapacity}\nStatus: {status} ";
    }

    public override void GetInfo()
    {
        Console.WriteLine($"Id: {Id} Model: {Model} Airline: {Airline}");
    }

    public override void GetGridInfo()
    {
        Console.Write($"Id: {Id,-5} Model: {Model,-20} Airline: {Airline,-20} Age: {AirplaneAge, -5}, Engines type: {EnginesType, -10} Capacity: {Capacity, -5}, FirstClass Capacity: {FirstClassCapacity}");
    }

    public void Board(int[] passengers)
    {
        Boarded = true;
        Console.WriteLine($"Airplane {Id} boarded");
        Passengers = passengers;
    }

    public void BoardStatus()
    {
        if(Passengers == null) return;
        Console.WriteLine("====================");
        Console.WriteLine($"{Model} id: {Id}");
        Console.WriteLine($"Turist Class: {Passengers[0]}\nFirst Class:{Passengers[1]}\n");
    }

}