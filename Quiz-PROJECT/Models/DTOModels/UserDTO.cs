using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Quiz_PROJECT.Models.Validators;

namespace Quiz_PROJECT.Models.DTOModels;

public class UserDTO : BaseModelDTO
{
    [Required]
    public string FirstName { get; set; }

    public string? LastName { get; set; }
    
    public string FullName => String.IsNullOrEmpty(LastName) ? FirstName : $"{FirstName} {LastName}";

    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email")]
    [UserValidation(ErrorMessage = "User with this email has already exist!")]
    public string Email { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    [UserValidation(ErrorMessage = "User with this phone number has already exist!")]
    public string Phone { get; set; }

    [Range(0, 125)]
    public int? Age { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(24)")]
    public Role Role { get; set; }
}

public class CreateUserDTO
{
    [Required]
    public string FirstName { get; set; }

    public string? LastName { get; set; }
    
    public string FullName => String.IsNullOrEmpty(LastName)? FirstName : $"{FirstName} {LastName}";

    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email")]
    [UserValidation(ErrorMessage = "User with this email has already exist!")]
    public string Email { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    [UserValidation(ErrorMessage = "User with this phone number has already exist!")]
    public string Phone { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    [Range(0, 125)] 
    public int? Age { get; set; }
}

public class UpdateUserDTO
{
    [Required]
    public string FirstName { get; set; }

    public string? LastName { get; set; }
    
    public string FullName => String.IsNullOrEmpty(LastName)? FirstName : $"{FirstName} {LastName}";

    [Required]
    public string Username { get; set; }

    [Required]
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

    [Range(0, 125)] 
    public int? Age { get; set; }
}