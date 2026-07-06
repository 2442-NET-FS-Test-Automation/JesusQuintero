using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Entities;

public class MaterialsByShipments
{
    [Key]
    public int MaterialsByShioment_Id {get; set; }

    [Required]
    public int Shipment_Id {get; set; }

    public Shipments Shipment {get; set; } = default!;

    [Required]
    public int Material_Id {get; set; }

    public Materials Material {get; set; } = default!;

    [Required, Range(1, int.MaxValue)]
    public int Quantity {get; set; }
    
    public byte[] RowVersion {get; set; } = default!;
}