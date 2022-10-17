using System.ComponentModel.DataAnnotations;

namespace Quiz_PROJECT.Models.DTOModels;

public class AuthLoginUserDTO
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Must be at least 6 symbols")]
    public string Password { get; set; }
}