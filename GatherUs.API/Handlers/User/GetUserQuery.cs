using CSharpFunctionalExtensions;
using GatherUs.Core.UserLogic;
using MediatR;

namespace GatherUs.API.Handlers.User;

public record GetUserQuery(int Id) : IRequest<Result<int>>;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<int>>
{
    private readonly IUserService _userService;
    
    public GetUserQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public Task<Result<int>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        if (_userService.CreateUser(request.Id).IsFailure)
        {
            return Task.FromResult(Result.Failure<int>("Invalid Id"));
        }

        return Task.FromResult(Result.Success(request.Id));
    }
}