using System.ComponentModel.DataAnnotations;

namespace Quiz_PROJECT.Models.Validators;

public class UserValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var _dbContext = (DBContext)validationContext.GetService(typeof(DBContext));

        if (_dbContext.Users.FirstOrDefault(user => user.Phone == value.ToString() ||
                                                    user.Email == value.ToString()) is null)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage ?? "User with this Email or Phone number already exist!");
    }
}