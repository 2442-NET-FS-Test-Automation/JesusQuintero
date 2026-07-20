using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Data.Entities;

[Table("Bins")]
public class Bins
{
    [Key]
    public int Bin_Id {get; set; }
    public string Bin_Name {get; set;} = "";
    [Required]
    public int Location_Id {get; set; }
    public Locations Location {get; set; } = default!;
    [Required]
    public bool RealBin {get; set; } = false;

    public DateOnly CreationDate {get; set; }
    public ICollection<LocatedMaterials> LocatedMaterials { get; set; } = default!;
}