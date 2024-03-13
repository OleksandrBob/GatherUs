using CSharpFunctionalExtensions;
using GatherUs.Core.Services.Interfaces;
using MediatR;

namespace GatherUs.API.Handlers.Users;

public class UploadUserPictureCommand : IRequest<Result<string>>
{
    internal int CurrentUserId { get; set; }
    
    public string Picture { get; set; }
    
    public string Name { get; set; }

    public class Handler : IRequestHandler<UploadUserPictureCommand, Result<string>>
    {
        private readonly IUserService _userService;

        public Handler(IUserService userService)
        {
            _userService = userService;
        }
        
        public async Task<Result<string>> Handle(UploadUserPictureCommand request, CancellationToken cancellationToken)
        {
            return await _userService.UploadProfilePicture(request.CurrentUserId, request.Picture, request.Name);
        }
    }
}