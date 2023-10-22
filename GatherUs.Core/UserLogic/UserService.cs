using CSharpFunctionalExtensions;

namespace GatherUs.Core.UserLogic;

public interface IUserService
{
    Result CreateUser(int userId);
}

public class UserService : IUserService
{
    public UserService()
    {
    }

    public Result CreateUser(int userId)
    {
        return Result.Success();
    }
}