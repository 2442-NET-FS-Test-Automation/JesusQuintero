namespace Airport.Domain;

public class MilitaryAirplane : Airplanes
{
    private string Status {get; set;}
    public MilitaryAirplane(string status, string model, int airplaneAge, string engineModel, int engineCount, int capacity = -1) 
                            : base(model, airplaneAge, engineModel, engineCount, capacity)
    {
        Status = status;
    }

    public override void GetGridInfo()
    {
        Console.WriteLine($"Id: {Id,-5} | Model: {Model,-20} | Status:{Status, -10} | Age: {AirplaneAge, -5}, | Engines type: {EnginesType, -10} | Capacity{Capacity}");
    }

    public override void GetInfo()
    {
        Console.WriteLine($"Id: {Id} Model: {Model} Status: {Status}");
    }

    public void ChangeStatus(string newStatus)
    {
         Status = newStatus;
    }
}