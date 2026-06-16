using System.Runtime.CompilerServices;

namespace Airport.Domain;

public class CommercialAirplane : Airplanes
{
    public int? FirstClassCapacity {get; set;} 
    public string Airline {get; set;}


    public CommercialAirplane (string model, int capacity, int maxAltitude, int maxSpeed, int firstClassCapacity, string airline) 
                                : base (model, capacity, maxAltitude, maxSpeed)
    {
        FirstClassCapacity = firstClassCapacity;
        Airline = airline;
    }

    public override string ToString()
    {
        string status;
        if (Status == false) {
            status = "Without charge";
        }
        else
        {
            status = "Charged";
        }
        return $"===== {Model} from {Airline}=====\nTurist Capacity: {Capacity - FirstClassCapacity}\nFirst Class: {FirstClassCapacity}\nStatus: {status} ";
    }

    public override void GetInfo()
    {
        Console.WriteLine($"Id: {Id} Model: {Model} Airline: {Airline}");
    }

    public override void ChargeAirplane()
    {
        Status = true;
        Console.WriteLine($"Airplane {Id} charged");
    }

}