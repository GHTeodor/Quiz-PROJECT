namespace Quiz_PROJECT.Models;

public class Question : BaseModel
{
    public string Title { get; set; }
    public ICollection<Answer>? Answers { get; set; }
}