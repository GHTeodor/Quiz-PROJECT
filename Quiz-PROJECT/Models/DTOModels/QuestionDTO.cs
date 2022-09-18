namespace Quiz_PROJECT.Models.DTOModels;

public class QuestionDTO : BaseModelDTO
{
    public string Title { get; set; }
    public ICollection<AnswerDTO>? Answers { get; set; }
}

public class CreateQuestionDTO
{
    public string Title { get; set; }
    public ICollection<Answer>? Answers { get; set; }
}

public class UpdateQuestionDTO
{
    public string Title { get; set; }
    public ICollection<Answer>? Answers { get; set; }
}

public class AnswerFields_QuestionDTO
{
    public string Answer_num { get; set; }
    public int QuestionId { get; set; }
}