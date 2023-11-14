using System.Linq.Expressions;
using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Models;
using GatherUs.DAL.Repository;

namespace GatherUs.Core.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<User> GetByEmailAsync(string email, params Expression<Func<User, object>>[] includes)
        => await _unitOfWork.Users.GetByAsync(c => c.Mail == email, includes: includes);   
}