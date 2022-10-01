using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quiz_PROJECT.Models;

public class RefreshToken : BaseModel
{
    public string Token { get; set; }
    
    public DateTimeOffset Expires { get; set; }
    
    [ForeignKey(nameof(User))]
    public long UserId { get; set; }
    
    [JsonIgnore]
    public User? User { get; set; }
}