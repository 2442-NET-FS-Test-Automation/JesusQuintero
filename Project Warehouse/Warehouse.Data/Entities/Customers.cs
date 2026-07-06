using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Data.Entities;

[Table("Customers")]
public class Customers
{
    [Key]
    public int Customer_Id {get; set; }

    [MaxLength(50)]
    public string Customer_Name {get; set; }

    [MaxLength(100)]
    public string Customer_Email {get; set; }

    public ICollection<Shipments> Shipments { get; set; } = default!;
}