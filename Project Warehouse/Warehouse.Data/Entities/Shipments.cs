using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data;

public class Shipments
{
    [Key]
    public int Shipment_Id {get; set; }

    [Required]
    public int Customer_Id {get; set; }
    public Customers Customer {get; set; } = default!;

    [Required]
    public DateOnly Shipment_Date {get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Required, Range(.1, float.MaxValue)]
    public float Sale_Price {get; set; }

    [Required]
    public int Status {get; set; } = 0;
    public byte[] RowVersion {get; set; } = default!;
}