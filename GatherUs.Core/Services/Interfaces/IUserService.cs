using System.Linq.Expressions;
using GatherUs.DAL.Models;

namespace GatherUs.Core.Services.Interfaces;

public interface IUserService
{
    Task<User> GetByEmailAsync(string email, params Expression<Func<User, object>>[] includes);

    Task<User> GetByIdAsync(int id, params Expression<Func<User, object>>[] includes);
}