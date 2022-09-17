﻿using System.ComponentModel.DataAnnotations;

namespace Quiz_PROJECT.Models;

public class BaseModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
}