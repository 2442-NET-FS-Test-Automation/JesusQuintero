namespace Library.Data.Entities;

public class FulfillmentEvent
{
    public int Id {get; set; }
    public int OrderId {get; set; }

    // = defailt! is something we're doing for EF Core. If we were to make this nullable we'd
    // satisfy the compiler - but what if I DONT want the database column to allow null?
    public string Type {get; set; } = default!;
    public DateTime FulfilledAtUtc {get; set; } = DateTime.UtcNow;
}