namespace Library.ControllerApi.Services;

public class SupplierClient : ISupplierClient
{
    // This class will call an outside API using HTTP Client
    private readonly HttpClient _http; // comes from ASP.NET DI Container

    public SupplierClient(HttpClient http)
    {
        _http = http;
    }

    // Record to represent the response "shapr" of that  outside API
    private record SupplierProduct(int Id, string Title, decimal Price);

    // This method seds a GET to a tracking API called dummyJson
    // GET https://dummyjson.com/products/id -> This is live

    public async Task<decimal?> GetListPriceAsync(string sku)
    {
        // Lets pretend we are grabbing the  "Wholesale price" of our products from an applier
        var digits = new string(sku.Where(char.IsDigit).ToArray());// "BK-001 -> "001"

        // Check to make sure We don't have a null in digits
        if(!int.TryParse(digits, out var id)) return null; // If our string was empty, just return null

        // Appending the rest of the URL  to the base  URL  we set up with builder.Service
        var product = await _http.GetFromJsonAsync<SupplierProduct>($"products/{id}");

        return product?.Price;
    }
}