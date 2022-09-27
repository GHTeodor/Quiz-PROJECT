namespace Quiz_PROJECT.Models;

public class Question : BaseModel
{
    public string Category { get; set; }
    public string Type { get; set; }
    public string Difficulty { get; set; }
    public string TitleQuestion { get; set; }
    public string CorrectAnswer { get; set; }
    public ICollection<Answer> IncorrectAnswers { get; set; }
}