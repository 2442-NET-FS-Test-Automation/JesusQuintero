using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Data.Entities;

[Table("Locations")]
public class Locations
{
    [Key]
    public int Location_Id {get; set; }
    
    [Required, MaxLength(20)]
    public string Location_Name {get; set; }
}
