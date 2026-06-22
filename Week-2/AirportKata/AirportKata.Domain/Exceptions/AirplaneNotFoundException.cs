namespace Airport.Domain;

public class AirplaneNotFoundException : AirplaneException
{
    public int Id {get; }

    public AirplaneNotFoundException(int id) 
            : base($"No Airpnalne within id {id}")
    {
        Id = id;
    }
}