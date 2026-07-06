using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Entities;

public class MaterialsByModels
{
    [Key]
    public int MaterialByModel_Id {get; set; }

    [Required]
    public int Model_Id {get; set; }

    public Models model {get; set; }

    [Required]
    public int Material_Id {get; set; }

    public Materials Material {get; set; }
    
    [Required, Range(1,int.MaxValue)]
    public int Quantity {get; set; }
}