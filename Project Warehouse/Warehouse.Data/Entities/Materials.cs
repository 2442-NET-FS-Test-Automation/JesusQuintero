using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Entities;

public class Materials
{
    [Key]
    public int Material_Id {get; set; }

    [Required, MaxLength(50)]
    public string Material_Name {get; set; }

    public string Material_Description {get; set; }

    public int Vendor_Id {get; set; }

    public Vendors vendor {get; set; } = default!;
    
    public byte[] RowVersion {get; set; } = default!;

    public ICollection<LocatedMaterials> LocatedMaterials { get; set; } = default!;
    public ICollection<MaterialMovements> MaterialMovements { get; set; } = default!;
    public ICollection<MaterialsByModels> MaterialsByModels { get; set; } = default!;
    public ICollection<Models> Models { get; set; } = default!;
    public ICollection<MaterialsByShipments> MaterialsByShipments { get; set; } = default!;
}
