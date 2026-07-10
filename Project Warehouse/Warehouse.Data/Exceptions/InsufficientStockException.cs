namespace Warehouse.Data;

public sealed class InsufficientStockException : Exception
{
    public int _bin {get; }
    public InsufficientStockException(int bin) : base($"Not enought stock in bin {bin}"){ _bin = bin; }
}