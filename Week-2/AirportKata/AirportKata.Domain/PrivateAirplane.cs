using Serilog;

namespace Airport.Domain;

public class PrivateAirplane : Airplanes, IBoardable
{
    

    public string Owner {get; set;}
    public string Status {get; set;}
    public int Passengers {get; set;}


    public PrivateAirplane(string owner, string status, string model, int airplaneAge, string engineModel, int engineCount, int capacity = -1) 
                            : base(model, airplaneAge, engineModel, engineCount, capacity)
    {
        Owner = owner;
        Status = status;
        Passengers = 0;
    }

    public override void GetGridInfo()
    {
        Console.WriteLine($"Id: {Id,-5} | Owner: {Owner,-20} | Status:{Status, -16} | Capacity{Capacity}");
        Console.WriteLine($"Age: {AirplaneAge, -5},| Model: {Model,-20} | Engines type: {EnginesType, -10} | Passengers: {Passengers}");
    }

    public override void GetInfo()
    {
        Console.WriteLine($"Id: {Id} Model: {Model} Status: {Status}");
    }

    public void Board(int[] passengers)
    {
        Passengers = passengers[0];
        Status = "Boarded";
        Log.Information("Airplane {Id} boarded", Id);
    }

    public void BoardStatus()
    {
        Console.WriteLine("====================");
        Console.WriteLine($"{Model} id: {Id}");
        if(Passengers > 0 ) Console.WriteLine($"Status: boarded with {Passengers} passengers\n");
        Console.WriteLine($"Status: {Status}\n");
    }
}