﻿using System.ComponentModel.DataAnnotations;
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

    [Range(0, 125)] public int? Age { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(24)")] 
    public Role Role { get; set; } = Role.USER;
}