using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Entities;

public class Vendors
{
    [Key]
    public int Vendor_Id {get; set; }

    [Required, MaxLength(50)]
    public string Vendor_Name {get; set; }

    [MaxLength(100)]
    public string Vendor_Email {get; set; }
}