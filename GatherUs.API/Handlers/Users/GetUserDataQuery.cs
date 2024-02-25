using AutoMapper;
using CSharpFunctionalExtensions;
using GatherUs.API.DTO.User;
using GatherUs.Core.Errors;
using GatherUs.Core.Services.Interfaces;
using MediatR;

namespace GatherUs.API.Handlers.Users;

public class GetUserDataQuery : IRequest<Result<UserDto, FormattedError>>
{
    public int UserId { get; set; }

    public class Handler : IRequestHandler<GetUserDataQuery, Result<UserDto, FormattedError>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public Handler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<Result<UserDto, FormattedError>> Handle(GetUserDataQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(request.UserId);

            if (user is null)
            {
                return Result.Failure<UserDto, FormattedError>(new("User with specified id doesn't exist"));
            }

            return Result.Success<UserDto, FormattedError>(_mapper.Map<UserDto>(user));
        }
    }
}
