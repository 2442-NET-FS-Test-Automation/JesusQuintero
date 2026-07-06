namespace Warehouse.Data;

public class MaterialsByModels
{
    public int MaterialByModel_Id {get; set; }
    public int Model_Id {get; set; }
    public Models model {get; set; }
    public int Material_Id {get; set; }
    public Materials Material {get; set; }
    public int Quantity {get; set; }
}