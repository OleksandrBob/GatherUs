using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using GatherUs.Core.Errors;
using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Models;
using GatherUs.Enums.DAL;
using MediatR;

namespace GatherUs.API.Handlers.Accounts;

public class SignUpCommand : IRequest<Result<object, FormattedError>>
{
    [Required]
    [EmailAddress(ErrorMessage = "Email is invalid")]
    public string Mail { get; set; } = null!;

    public UserType? Role { get; set; }

    public class Handler : IRequestHandler<SignUpCommand, Result<object, FormattedError>>
    {
        private readonly IUserService _userService;
        private readonly IGuestService _guestService;
        private readonly IOrganizerService _organizerService;

        public Handler(
            IUserService userService,
            IGuestService guestService,
            IOrganizerService organizerService)
        {
            _userService = userService;
            _guestService = guestService;
            _organizerService = organizerService;
        }

        public async Task<Result<object, FormattedError>> Handle(SignUpCommand request,
            CancellationToken cancellationToken)
        {
            User user = await _userService.GetByEmailAsync(request.Mail);

            if (user is not null)
            {
                if (user.UserType != request.Role)
                {
                    return Result.Failure<object, FormattedError>(
                        new()
                        {
                            ErrorMessage =
                                $"User with this mail has already registered as a {user.UserType}",
                            Args = new() { user.UserType.ToString() },
                        });
                }
            }

            return request.Role switch
            {
                UserType.Guest => await RegisterGuest(request.Mail, user),
                UserType.Organizer => await RegisterOrganizer(request.Mail, user),
                _ => Result.Failure<object, FormattedError>(new("Cannot specify user type"))
            };
        }

        private async Task<Result<object, FormattedError>> RegisterGuest(string requestMail, User alreadyRegisteredUser)
        {
            if (alreadyRegisteredUser?.IsMailConfirmed == true)
            {
                return Result.Failure<object, FormattedError>(new("User with specified mail is already registered"));
            }

            var guestToInsert = new Guest
            {
                UserType = UserType.Guest,
                Mail = requestMail,
            };
            
            await _guestService.InsertAsync(guestToInsert);
            throw new NotImplementedException();
        }

        private async Task<Result<object, FormattedError>> RegisterOrganizer(string requestMail,
            User alreadyRegisteredUser)
        {
            if (alreadyRegisteredUser?.IsMailConfirmed == true)
            {
                return Result.Failure<object, FormattedError>(new("User with specified mail is already registered"));
            }
            throw new NotImplementedException();
        }
    }
}
