using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using GatherUs.Core.Errors;
using GatherUs.Core.Helpers;
using GatherUs.Core.Services.Interfaces;
using MediatR;

namespace GatherUs.API.Handlers.Accounts;

public class SignInCommand : IRequest<Result<string, FormattedError>>
{
    [Required]
    [EmailAddress(ErrorMessage = "Email is invalid")]
    public string Mail { get; set; }

    [Required]
    [MinLength(8)]
    [MaxLength(20)]
    public string Password { get; set; }

    public class Handler : IRequestHandler<SignInCommand, Result<string, FormattedError>>
    {
        private readonly IUserService _userService;
        private readonly ITokenManager _tokenManager;

        public Handler(IUserService userService, ITokenManager tokenManager)
        {
            _userService = userService;
            _tokenManager = tokenManager;
        }

        public async Task<Result<string, FormattedError>> Handle(SignInCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userService.GetByEmailAsync(request.Mail);
            if (user is null)
            {
                return Result.Failure<string, FormattedError>(
                    new()
                    {
                        ErrorMessage =
                            $"User with this mail is not registered",
                    });
            }

            var hashedPass = CryptoHelper.GenerateSaltedHash(request.Password);

            if (user.Password != hashedPass)
            {
                return Result.Failure<string, FormattedError>(
                    new()
                    {
                        ErrorMessage =
                            "Wrong password",
                    });
            }

            var token = _tokenManager.GenerateJwt(user);

            return Result.Success<string, FormattedError>(token);
        }
    }
}
