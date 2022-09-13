using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz_PROJECT.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string NickName { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    public string Phone { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    [Range(0, 125)] public int? Age { get; set; }

    [Column(TypeName = "nvarchar(24)")] 
    public Role Role { get; set; } = Role.USER;
}