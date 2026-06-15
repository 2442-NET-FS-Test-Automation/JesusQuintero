namespace Airport.Domain;

public abstract class Airships
{
    public string? Model {get; private set;}
    public int? Capacity {get; private set;}
    public int? MaxAltitude {get; private set;}
    public float MaxSpeed {get; private set;}
    protected int id {get; private set;}

    private static int _nextId = 1;

    protected Airships(string model, int capacity, int maxAltitude, float maxSpeed)
    {
        id = _nextId++;
        Model = model;
        Capacity = capacity;
        MaxAltitude = maxAltitude;
        MaxSpeed = maxSpeed;
    } 


} 