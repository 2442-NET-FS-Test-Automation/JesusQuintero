namespace Warehouse.Data;

public class Models
{
    public int Model_Id {get; set; }
    public string Model_Name {get; set; }
    public int New_Material_Id {get; set; }
    public Materials New_Material {get; set; } = default!;
}