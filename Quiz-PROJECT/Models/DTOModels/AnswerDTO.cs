namespace Quiz_PROJECT.Models.DTOModels;

public class AnswerDTO : BaseModelDTO
{
    public string Answer_num { get; set; }

    public string Title { get; set;}
    
    public int QuestionId { get; set; }
    
    public QuestionDTO Question { get; set; }
}

public class CreateAnswerDTO
{
    public string Answer_num { get; set; }
    
    public int QuestionId { get; set; }

    public Question? Question { get; set; }
}

public class UpdateAnswerDTO
{
    public string Answer_num { get; set; }
    
    public int QuestionId { get; set; }

    public Question? Question { get; set; }
}

public class QuestionFields_AnswerDTO
{
    public string Title { get; set; }
}