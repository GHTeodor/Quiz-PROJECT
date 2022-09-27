using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Quiz_PROJECT.Models.Validators;

public class UserEmailPhoneUniqueValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        DBContext _dbContext = (DBContext)validationContext.GetService(typeof(DBContext))!;

        if (_dbContext.Users.FirstOrDefault(user => user.Email.ToUpper() == value.ToString()!.ToUpper() ||
                                                     user.Phone == value.ToString()) is null)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage ?? "User with this Email or Phone number already exist!");
    }
}