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
    public string Mail { get; set; }

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

    public short ConfirmationNumber { get; set; }

    public class Handler : IRequestHandler<SignUpCommand, Result<object, FormattedError>>
    {
        private readonly IUserService _userService;
        private readonly IGuestService _guestService;
        private readonly IMailingService _mailingService;
        private readonly IOrganizerService _organizerService;
        private readonly IEmailForRegistrationService _emailForRegistrationService;

        public Handler(
            IUserService userService,
            IGuestService guestService,
            IMailingService mailingService,
            IOrganizerService organizerService,
            IEmailForRegistrationService emailForRegistrationService)
        {
            _userService = userService;
            _guestService = guestService;
            _mailingService = mailingService;
            _organizerService = organizerService;
            _emailForRegistrationService = emailForRegistrationService;
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

            var emailForRegistration = await _emailForRegistrationService.GetEmailForRegistrationAsync(request.Mail);
            if (emailForRegistration is null)
            {
                return Result.Failure<object, FormattedError>(
                    new()
                    {
                        ErrorMessage =
                            $"Registration request was not sent to email: {request.Mail}",
                        Args = new() { request.Mail },
                    });
            }

            if (request.ConfirmationNumber != emailForRegistration.ConfirmationCode)
            {
                return Result.Failure<object, FormattedError>(
                    new()
                    {
                        ErrorMessage =
                            $"Wrong confirmation code",
                        Args = new() { request.Mail },
                    });
            }

            var signUpResult = emailForRegistration.UserType switch
            {
                UserType.Guest => await RegisterGuest(request),
                UserType.Organizer => await RegisterOrganizer(request),
                _ => Result.Failure<object, FormattedError>(new("Cannot specify user type"))
            };

            if (signUpResult.IsSuccess)
            {
                await _emailForRegistrationService.RemoveEmailForRegistrationAsync(emailForRegistration.Id);
            }

            return signUpResult;
        }

        private async Task<Result<object, FormattedError>> RegisterGuest(SignUpCommand request)
        {
            var guestToInsert = new Guest
            {
                Mail = request.Mail,
                LastName = request.LastName,
                Password = request.Password,
                FirstName = request.FirstName,
                LoginProvider = LoginProvider.GatherUs,
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
                LoginProvider = LoginProvider.GatherUs,
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
