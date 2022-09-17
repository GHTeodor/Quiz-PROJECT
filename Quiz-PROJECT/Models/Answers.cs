using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz_PROJECT.Models;

public class Answers : BaseModel
{
    public string Answer_num { get; set; }
    
    [ForeignKey(nameof(Question))]
    public int QuestionId { get; set; }
    public Question? Question { get; set; }

}