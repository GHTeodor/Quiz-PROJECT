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

    public async Task<IEnumerable<UserDTO>> GetAllAsync(CancellationToken token = default)
    {
        return _mapper.Map<IEnumerable<UserDTO>>(await _unitOfWork.Users.GetAllAsync(token));
    }
    
    public async Task<User> GetByIdAsync(long id, CancellationToken token = default)
    {
        return await _unitOfWork.Users.GetByIdAsync(id, token);
    }

    public async Task DeleteByIdAsync(long id, CancellationToken token = default)
    {
        await _unitOfWork.Users.DeleteByIdAsync(id, token);
        await _unitOfWork.SaveAsync(token);
        await _unitOfWork.DisposeAsync();
    }
}