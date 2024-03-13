using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using GatherUs.DAL.Models;
using Microsoft.AspNetCore.Http;

namespace GatherUs.Core.Services.Interfaces;

public interface IUserService
{
    Task<User> GetByEmailAsync(string email, params Expression<Func<User, object>>[] includes);

    Task<User> GetByIdAsync(int id, params Expression<Func<User, object>>[] includes);

    Task<Result> DeleteProfilePicture(int userId);

    Task<Result<string>> UploadProfilePicture(int userId, string imageFile, string name);
}