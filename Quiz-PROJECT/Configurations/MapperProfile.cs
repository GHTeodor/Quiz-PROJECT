using AutoMapper;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;

namespace Quiz_PROJECT.Configurations;

public class MapperInitializer : Profile
{
    public MapperInitializer()
    {
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<User, CreateUserDTO>().ReverseMap();
        CreateMap<User, UpdateUserDTO>().ReverseMap();

        // CreateMap<Question, QuestionDTO>();
        // CreateMap<Question, CreateQuestionDTO>();
        // CreateMap<Question, UpdateQuestionDTO>();
        
        // CreateMap<Answers, AnswerDTO>();
        // CreateMap<Answers, CreateAnswerDTO>();
        // CreateMap<Answers, UpdateAnswerDTO>();
    }
}