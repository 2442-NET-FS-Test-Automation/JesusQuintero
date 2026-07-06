namespace Warehouse.Data;

public class Shipments
{
    public int Shipment_Id {get; set; }
    public int Customer_Id {get; set; }
    public Customers Customer {get; set; } = default!;
    public DateOnly Shipment_Date {get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public float Sale_Price {get; set; }
    public int Status {get; set; }
    public byte[] RowVersion {get; set; } = default!;
}