using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Serilog;


namespace Airport.Domain;
public class OpenAircraftClient
{
    private static readonly HttpClient client = new();
    private static readonly int Limit = 1;

    public async Task<Airplanes?> FetchByIdAsync(string isbn)
    {
        string url = $"https://api.aviationstack.com/v1/airplanes?access_key={isbn}&limit={Limit}";

        try
        {
            string jsonResponse = await client.GetStringAsync(url);
            
            return Parse(jsonResponse);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Exception trying to connet with API: {ex.Message}");
            Log.Warning("Exception trying to connet with API: {Message}", ex.Message);
            return null;
        }
    }

    public static Airplanes? Parse(string json)
    {
        Dictionary<string, JsonElement>? resp = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

        if(resp is null || !resp.TryGetValue("data", out JsonElement data) || data.GetArrayLength() == 0)
            return null;
        


        JsonElement foundAirplane = data[0];

        string model = foundAirplane.GetProperty("production_line").GetString() ?? "Unknown";
        string airline = foundAirplane.GetProperty("plane_owner").GetString() ?? "Unknown";
        string age = foundAirplane.GetProperty("plane_age").GetString() ?? "Unknown";
        string engineType = foundAirplane.GetProperty("engines_type").GetString() ?? "Unknown";
        string engineCount = foundAirplane.GetProperty("engines_count").GetString() ?? "Unknown";

        Console.Clear();
        Console.WriteLine($"API Information: \nModel {model}\n{airline}");
        
        

        return AirplaneFactory.Create(ItemKind.CommercialAirplane, model, int.Parse(age), engineType, int.Parse(engineCount));
    }
}