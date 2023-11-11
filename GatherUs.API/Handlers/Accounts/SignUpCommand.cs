using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using GatherUs.Core.Errors;
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

        public Handler()
        {

        }

        public async Task<Result<object, FormattedError>> Handle(SignUpCommand request,
            CancellationToken cancellationToken)
        {
            return Result.Success();
        }
    }
}