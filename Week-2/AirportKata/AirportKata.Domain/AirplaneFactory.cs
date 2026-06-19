namespace Airport.Domain;

public static class AirplaneFactory
{
    public static Airplanes Create(
        ItemKind kind,
        string model,
        int airplaneAge,
        string engineType,
        int engineCount,
        int capacity = 0,
        int firstclasscapacity = 0,
        string airline = "Unknown"
    )
    {
        switch (kind)
        {
            case ItemKind.CommercialAirplane:
                return new CommercialAirplane(model, airplaneAge, engineType, engineCount, airline, firstclasscapacity, capacity);
            default:
                throw new Exception();

        }
    }
}