namespace Quiz_PROJECT.Models.DTOModels;

public class QuestionDTO : BaseModelDTO
{
    public string Title { get; set; }
    public ICollection<AnswerDTO>? Answers { get; set; }
}

public class CreateQuestionDTO
{
    
}

public class UpdateQuestionDTO
{
    
}