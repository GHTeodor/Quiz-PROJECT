using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz_PROJECT.Models;

public class Answers
{
    public int Id { get; set; }
    public string Answer_num { get; set; }
    public Question? Question { get; set; }
    
    [ForeignKey(nameof(Question))]
    public int? QuestionId { get; set; }
}