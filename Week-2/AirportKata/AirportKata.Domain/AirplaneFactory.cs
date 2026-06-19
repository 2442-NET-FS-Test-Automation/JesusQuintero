namespace Airport.Domain;

public static class AirplaneFactory
{
    public static Airplanes Create(
        ItemKind kind,
        string model,
        int capacity,
        int max_altitude,
        int max_speed,
        int firstclasscapacity = 0,
        string airline = "Unknown"
    )
    {
        switch (kind)
        {
            case ItemKind.CommercialAirplane:
                return new CommercialAirplane(model, capacity,max_altitude,max_speed,firstclasscapacity,airline);
            default:
                throw new Exception();

        }
    }
}