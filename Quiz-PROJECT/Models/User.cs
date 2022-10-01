using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Quiz_PROJECT.Models.Validators;

namespace Quiz_PROJECT.Models;

public class User : BaseModel
{
    [Required]
    public string FirstName { get; set; }

    public string? LastName { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email")]
    [UserEmailPhoneUniqueValidation(ErrorMessage = "You can't use this email. User with this email has already exist!")]
    public string Email { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    [UserEmailPhoneUniqueValidation(ErrorMessage = "You can't use this phone number. User with this phone number has already exist!")]
    [MaxLength(18, ErrorMessage = "Max length 18 symbols")]
    public string Phone { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public byte[] PasswordHash { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public byte[] ConfirmPasswordHash { get; set; }
    
    public byte[] PasswordSalt { get; set; }

    [Range(0, 125)] public byte? Age { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(24)")] 
    public Role Role { get; set; } = Role.USER;
    
    public RefreshToken RefreshToken { get; set; }
}