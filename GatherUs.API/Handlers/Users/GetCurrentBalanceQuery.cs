using GatherUs.Core.Services.Interfaces;
using CSharpFunctionalExtensions;
using GatherUs.Core.Errors;
using MediatR;

namespace GatherUs.API.Handlers.Users;

public class GetCurrentBalanceQuery : IRequest<Result<decimal, FormattedError>>
{
    internal int UserId { get; set; }

    public class Handler : IRequestHandler<GetCurrentBalanceQuery, Result<decimal, FormattedError>>
    {
        private readonly IUserService _userService;

        public Handler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<decimal, FormattedError>> Handle(GetCurrentBalanceQuery request, CancellationToken ct)
        {
            var user = await _userService.GetByIdAsync(request.UserId);

            if (user is null)
            {
                return Result.Failure<decimal, FormattedError>(new("User with specified id is not found"));
            }

            return Result.Success<decimal, FormattedError>(user.Balance);
        }
    }
}
