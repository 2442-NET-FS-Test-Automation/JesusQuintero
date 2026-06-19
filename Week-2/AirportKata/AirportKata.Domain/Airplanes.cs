namespace Airport.Domain;

// Class to be base for more type of Airships
public abstract class Airplanes
{
    public string? Model {get; private set;}
    public int Capacity {get; private set;}

    public int AirplaneAge;
    public string EnginesModel;
    public int EnginesCount;

    // public int? MaxAltitude {get; private set;}
    // public int MaxSpeed {get; private set;}
    public int Id {get; private set;}
    private static int _nextId = 1;

    protected Airplanes(string model, int airplaneAge, string engineModel, int engineCount, int capacity = -1)
    {
        Id = _nextId++;
        Model = model;
        Capacity = capacity;
        AirplaneAge = airplaneAge;
        EnginesModel = engineModel;
        EnginesCount = engineCount;
        // MaxAltitude = maxAltitude;
        // MaxSpeed = maxSpeed;
    } 

    // Get information to manage flights
    public abstract void GetInfo();

} 