using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Data.Entities;

[Table("MaterialMovements")]
public class MaterialMovements
{
    [Key]
    public int MaterialMovement_Id {get; set; }

    [Required]
    public int Movement_Id {get; set; }

    public Movements Movement {get; set; } = default!;
    
    [Required]
    public int Material_Id {get; set; }

    public Materials Material {get; set; } = default!;

    public byte[] RowVersion {get; set; } = default!;
}