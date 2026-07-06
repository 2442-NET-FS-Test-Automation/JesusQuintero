using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Data.Entities;

[Table("LocatedMaterialas")]
public class LocatedMaterials
{
    [Key]
    public int LocatedMaterials_Id {get; set; }

    [Required]
    public int Bin_Id {get; set; }

    public Bins Bin {get; set; } = default!;

    [Required]
    public int Material_Id {get; set; }

    public Materials Material {get; set; } = default!;

    [Required, Range(1, int.MaxValue)]
    public int Quantity {get; set; } 
    
    public byte[] RowVersion {get; set; } = default!;
}