using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data;

public class Movements
{
    [Key]
    public int Movement_Id {get; set; }

    [Required]
    public int User_Id {get; set; }
    public Users User {get; set; } = default!;

    [Required]
    public DateTime Movement_Time {get; set; } = DateTime.Now;
    public int LastBinLocation_Id {get; set; }
    public Bins LastBinLocation {get; set; } = default!;
    public int NewBinLocation_Id {get; set; }
    public Bins NewBinLocation {get; set; } = default!;

}