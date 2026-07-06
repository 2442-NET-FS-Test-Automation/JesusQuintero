namespace Warehouse.Data;

public class MaterialsByShipments
{
    public int MaterialsByShioment_Id {get; set; }
    public int Shipment_Id {get; set; }
    public Shipments Shipment {get; set; } = default!;
    public int Material_Id {get; set; }
    public Materials Material {get; set; } = default!;
    public int Quantity {get; set; }
    public byte[] RowVersion {get; set; } = default!;
}