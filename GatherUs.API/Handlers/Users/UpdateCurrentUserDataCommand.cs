using CSharpFunctionalExtensions;
using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Repository;
using MediatR;

namespace GatherUs.API.Handlers.Users;

public class UpdateCurrentUserDataCommand : IRequest<Result>
{
    internal int UserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
    
    public class Handler : IRequestHandler<UpdateCurrentUserDataCommand, Result>
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUserService userService, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateCurrentUserDataCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(request.UserId);

            if (user is null)
            {
                return Result.Failure("User with specified Id doesn't exist");
            }
            
            user.LastName = request.LastName;
            user.FirstName = request.FirstName;
            
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();
            
            return Result.Success();
        }
    }
}