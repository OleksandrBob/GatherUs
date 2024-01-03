using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using GatherUs.Core.Errors;
using GatherUs.Core.Mailing;
using GatherUs.Core.Services.Interfaces;
using GatherUs.Enums.DAL;
using MediatR;

namespace GatherUs.API.Handlers.Accounts;

public class GetRegistrationConfirmationCodeCommand : IRequest<Result<object, FormattedError>>
{
    [Required]
    [EmailAddress(ErrorMessage = "Email is invalid")]
    public string Mail { get; set; }

    [Required] public UserType UserType { get; set; }

    public class Handler : IRequestHandler<GetRegistrationConfirmationCodeCommand, Result<object, FormattedError>>
    {
        private readonly IUserService _userService;
        private readonly IMailingService _mailService;
        private readonly IEmailForRegistrationService _emailForRegistrationService;

        public Handler(
            IUserService userService,
            IMailingService mailService,
            IEmailForRegistrationService emailForRegistrationService)
        {
            _userService = userService;
            _mailService = mailService;
            _emailForRegistrationService = emailForRegistrationService;
        }

        public async Task<Result<object, FormattedError>> Handle(GetRegistrationConfirmationCodeCommand request,
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
                var mailAddingResult =
                    await _emailForRegistrationService.AddEmailForRegistration(request.Mail, request.UserType);

                if (mailAddingResult.IsFailure)
                {
                    return Result.Failure(mailAddingResult.Error);
                }

                emailForRegistration = mailAddingResult.Value;
            }

            await _mailService.SendMailVerificationCodeAsync(emailForRegistration);

            return Result.Success();
        }
    }
}
