using CSharpFunctionalExtensions;
using GatherUs.Core.Services.Interfaces;
using MediatR;

namespace GatherUs.API.Handlers.Users;

public class RemoveUserPictureCommand : IRequest<Result>
{
    internal int CurrentUserId { get; set; }
    
    public class Handler : IRequestHandler<RemoveUserPictureCommand, Result>
    {
        private readonly IUserService _userService;

        public Handler(IUserService userService)
        {
            _userService = userService;
        }
        
        public async Task<Result> Handle(RemoveUserPictureCommand request, CancellationToken cancellationToken)
        {
            await _userService.DeleteProfilePicture(request.CurrentUserId);
            return Result.Success();
        }
    }
}