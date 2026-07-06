namespace Warehouse.Data;

public class LocatedMaterials
{
    public int LocatedMaterials_Id {get; set; }
    public int Bin_Id {get; set; }
    public Bins Bin {get; set; } = default!;
    public int Material_Id {get; set; }
    public Materials Material {get; set; } = default!;
    public int Quantity {get; set; } 
}