using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Data;

[Table("Customers")]
public class Customers
{
    [Key]
    public int Customer_Id {get; set; }

    [MaxLength(50)]
    public string Customer_Name {get; set; }

    [MaxLength(100)]
    public string Customer_Email {get; set; }
}