using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quiz_PROJECT.Models;

public class Answer : BaseModel
{
    public string IncorrectAnswer { get; set; }
    
    [ForeignKey(nameof(Question))] 
    public long QuestionId { get; set; }
    [JsonIgnore]
    public Question? Question { get; set; }
}