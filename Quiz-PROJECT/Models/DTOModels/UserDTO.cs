using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Quiz_PROJECT.Models.Validators;

namespace Quiz_PROJECT.Models.DTOModels;

public class UserDTO : BaseModelDTO
{
    [Required]
    public string FirstName { get; set; }

    public string? LastName { get; set; }
    
    public string FullName => string.IsNullOrEmpty(LastName) ? FirstName : $"{FirstName} {LastName}";

    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email")]
    [UserEmailPhoneUniqueValidation(ErrorMessage = "You can't use this email. User with this email has already exist!")]
    public string Email { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    [UserEmailPhoneUniqueValidation(ErrorMessage = "You can't use this phone number. User with this phone number has already exist!")]
    [MaxLength(18, ErrorMessage = "Max length 18 symbols")]
    public string Phone { get; set; }

    [Range(0, 125)]
    public byte? Age { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(24)")]
    public Role Role { get; set; }
}

public class CreateUserDTO
{
    [Required]
    public string FirstName { get; set; }

    public string? LastName { get; set; }
    
    public string FullName => string.IsNullOrEmpty(LastName)? FirstName : $"{FirstName} {LastName}";

    [Required]
    public string UserName { get; set; }

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
    [MinLength(6, ErrorMessage = "Must be at least 6 symbols")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    [Range(0, 125)] 
    public byte? Age { get; set; }
}

public class UpdateUserDTO
{
    [Required]
    public string FirstName { get; set; }

    public string? LastName { get; set; }
    
    public string FullName => string.IsNullOrEmpty(LastName)? FirstName : $"{FirstName} {LastName}";

    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    [MaxLength(18, ErrorMessage = "Max length 18 symbols")]
    public string Phone { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Must be at least 8 symbols")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    [Range(0, 125)] 
    public byte? Age { get; set; }
}