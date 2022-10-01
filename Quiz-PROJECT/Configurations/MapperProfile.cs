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

        CreateMap<RefreshToken, RefreshTokenDTO>().ReverseMap();

        CreateMap<Question, QuestionDTO>().ReverseMap();
        CreateMap<Question, CreateQuestionDTO>().ReverseMap();
        CreateMap<Question, UpdateQuestionDTO>().ReverseMap();
        
        CreateMap<Answer, AnswerDTO>().ReverseMap();
    }
}