namespace Warehouse.Data;

public class Materials
{
    public int Material_Id {get; set; }
    public string Material_Name {get; set; }
    public string Material_Description {get; set; }
    public int Vendor_Id {get; set; }
    public Vendors vendor {get; set; } = default!;
    public byte[] RowVersion {get; set; } = default!;
}
