using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using GatherUs.Core.Errors;
using GatherUs.Core.Mailing;
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

    [Required]
    [MinLength(2)]
    [MaxLength(15)]
    public string FirstName { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(15)]
    public string LastName { get; set; }

    [Required]
    [MinLength(8)]
    [MaxLength(20)]
    public string Password { get; set; }

    public UserType? Role { get; set; }

    public class Handler : IRequestHandler<SignUpCommand, Result<object, FormattedError>>
    {
        private readonly IUserService _userService;
        private readonly IGuestService _guestService;
        private readonly IMailingService _mailingService;
        private readonly IOrganizerService _organizerService;

        public Handler(
            IUserService userService,
            IGuestService guestService,
            IMailingService mailingService,
            IOrganizerService organizerService)
        {
            _userService = userService;
            _guestService = guestService;
            _mailingService = mailingService;
            _organizerService = organizerService;
        }

        public async Task<Result<object, FormattedError>> Handle(SignUpCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userService.GetByEmailAsync(request.Mail);

            if (user is not null)
            {
                return Result.Failure<object, FormattedError>(
                    new()
                    {
                        ErrorMessage =
                            $"User with this mail has already registered as a {user.UserType}",
                        Args = new() { user.UserType.ToString() },
                    });
            }

            return request.Role switch
            {
                UserType.Guest => await RegisterGuest(request),
                UserType.Organizer => await RegisterOrganizer(request),
                _ => Result.Failure<object, FormattedError>(new("Cannot specify user type"))
            };
        }

        private async Task<Result<object, FormattedError>> RegisterGuest(SignUpCommand request)
        {
            var guestToInsert = new Guest
            {
                Mail = request.Mail,
                UserType = UserType.Guest,
                LastName = request.LastName,
                Password = request.Password,
                FirstName = request.FirstName,
            };

            try
            {
                await _guestService.InsertAsync(guestToInsert);
                await _mailingService.SendGuestVerificationMailAsync(guestToInsert);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }

            return Result.Success();
        }

        private async Task<Result<object, FormattedError>> RegisterOrganizer(SignUpCommand request)
        {
            var organizerToInsert = new Organizer
            {
                Mail = request.Mail,
                LastName = request.LastName,
                Password = request.Password,
                FirstName = request.FirstName,
                UserType = UserType.Organizer,
            };

            try
            {
                await _organizerService.InsertAsync(organizerToInsert);
                await _mailingService.SendOrganizerVerificationMailAsync(organizerToInsert);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }

            return Result.Success();
        }
    }
}
