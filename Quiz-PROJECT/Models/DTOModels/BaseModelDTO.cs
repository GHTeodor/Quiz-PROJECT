using System.ComponentModel.DataAnnotations;

namespace Quiz_PROJECT.Models.DTOModels;

public class BaseModelDTO
{
    public int Id { get; set; }
    
    [Required]
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
}