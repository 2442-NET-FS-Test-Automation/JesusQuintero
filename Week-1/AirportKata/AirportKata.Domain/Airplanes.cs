namespace Airport.Domain;

// Class to be base for more type of Airships
public abstract class Airplanes
{
    public string? Model {get; private set;}
    public int? Capacity {get; private set;}
    public int? MaxAltitude {get; private set;}
    public float MaxSpeed {get; private set;}
    protected int Id {get; private set;}

    private static int _nextId = 1;

    protected Airplanes(string model, int capacity, int maxAltitude, float maxSpeed)
    {
        Id = _nextId++;
        Model = model;
        Capacity = capacity;
        MaxAltitude = maxAltitude;
        MaxSpeed = maxSpeed;
    } 

    // Get information to manage flights
    public abstract void GetInfo();
} 