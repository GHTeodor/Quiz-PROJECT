namespace Quiz_PROJECT.Models.DTOModels;

public class AnswerDTO : BaseModelDTO
{
    public string IncorrectAnswer { get; set; }
    public long QuestionId { get; set; }
}