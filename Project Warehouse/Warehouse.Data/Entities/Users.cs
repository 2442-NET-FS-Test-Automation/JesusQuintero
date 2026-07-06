using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data;

public class Users
{
    [Key]
    public int User_Id {get; set; }

    [Required, MaxLength(50)]
    public string User_Fullname {get; set; }

    [Required, MaxLength(100)]
    public string User_Adress {get; set; }

    [Required, MaxLength(100)]
    public string User_Email {get; set; }

    [Required]
    public string User_Password {get; set; }
}