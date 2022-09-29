using System.ComponentModel.DataAnnotations;

namespace Quiz_PROJECT.Models;

public class BaseModel
{
    [Key]
    public long Id { get; set; }
    
    [Required]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now.ToLocalTime();
    
    public DateTimeOffset? UpdatedAt { get; set; }
}