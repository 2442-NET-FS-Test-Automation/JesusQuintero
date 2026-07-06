namespace Warehouse.Data;

public class Bins
{
    public int Bin_Id {get; set; }
    public int Location_Id {get; set; }
    public Locations Location {get; set; } = default!;
    public bool RealBin {get; set; } = false;
}