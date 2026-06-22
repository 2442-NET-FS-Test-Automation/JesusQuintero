namespace Airport.Domain;

public class AirplaneException : Exception
{

    public AirplaneException(string message) : base (message){ }
}