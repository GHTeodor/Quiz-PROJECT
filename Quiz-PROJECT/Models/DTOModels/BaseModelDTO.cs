using System.ComponentModel.DataAnnotations;

namespace Quiz_PROJECT.Models.DTOModels;

public class BaseModelDTO
{
    [Key]
    public long Id { get; set; }
    
    public DateTimeOffset? CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
}