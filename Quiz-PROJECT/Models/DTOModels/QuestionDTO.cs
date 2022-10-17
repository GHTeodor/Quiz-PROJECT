namespace Quiz_PROJECT.Models.DTOModels;

public class QuestionDTO : BaseModelDTO
{
    public string Category { get; set; }
    public string Type { get; set; }
    public string Difficulty { get; set; }
    public string TitleQuestion { get; set; }
    public string CorrectAnswer { get; set; }
    public ICollection<AnswerDTO> IncorrectAnswers { get; set; }
}

public class CreateQuestionDTO
{
    public string Category { get; set; }
    public string Type { get; set; }
    public string Difficulty { get; set; }
    public string TitleQuestion { get; set; }
    public string CorrectAnswer { get; set; }
    public ICollection<AnswerDTO> IncorrectAnswers { get; set; }
}

public class UpdateQuestionDTO
{
    public string Category { get; set; }
    public string Type { get; set; }
    public string Difficulty { get; set; }
    public string TitleQuestion { get; set; }
    public string CorrectAnswer { get; set; }
    public ICollection<AnswerDTO> IncorrectAnswers { get; set; }
}