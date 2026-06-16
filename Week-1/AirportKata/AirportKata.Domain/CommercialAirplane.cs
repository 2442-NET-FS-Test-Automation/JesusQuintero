using System.Runtime.CompilerServices;

namespace Airport.Domain;

public class CommercialAirplane : Airplanes
{
    public int? FirstClassCapacity {get; set;} 
    public string Airline {get; set;}


    public CommercialAirplane (string model, int capacity, int maxAltitude, float maxSpeed, int firstClassCapacity, string airline) 
                                : base (model, capacity, maxAltitude, maxSpeed)
    {
        FirstClassCapacity = firstClassCapacity;
        Airline = airline;
    }

    public override string ToString()
    {
        return $"===== {Model} from {Airline}=====\nTurist Capacity: {Capacity - FirstClassCapacity}\nFirst Class: {FirstClassCapacity}";
    }

    public override void GetInfo()
    {
        Console.WriteLine($"Id: {Id} Model: {Model} Airline: {Airline}");
    }

}