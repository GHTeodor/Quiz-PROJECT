using AutoMapper;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;
using Quiz_PROJECT.UnitOfWork;

namespace Quiz_PROJECT.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(DBContext dbContext, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = new UnitOfWork.UnitOfWork(dbContext);
    }

    public async Task<IEnumerable<UserDTO>> GetAllAsync()
    {
        return _mapper.Map<IEnumerable<UserDTO>>(await _unitOfWork.Users.GetAllAsync());
    }
    
    public async Task<User> GetByIdAsync(long id)
    {
        return await _unitOfWork.Users.GetByIdAsync(id);
    }

    public async Task DeleteByIdAsync(long id)
    {
        await _unitOfWork.Users.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
        await _unitOfWork.DisposeAsync();
    }
}