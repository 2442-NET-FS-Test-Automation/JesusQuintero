using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Entities;

public class Models
{
    [Key]
    public int Model_Id {get; set; }

    [Required, MaxLength(50)]
    public string Model_Name {get; set; }

    [Required]
    public int New_Material_Id {get; set; }
    public Materials New_Material {get; set; } = default!;

    public ICollection<MaterialsByModels> materialsByModels { get; set; } = default!;
}