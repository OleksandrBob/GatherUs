using CSharpFunctionalExtensions;
using GatherUs.API.Handlers.Accounts;
using GatherUs.Core.Errors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GatherUs.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : Controller
{
    private readonly IMediator _mediator;
    
    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("get-confirmation-code")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRegistrationConfirmationCode(GetRegistrationConfirmationCodeCommand command)
    {
        Result<object, FormattedError> result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }
    
    [HttpPost("sign-up")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp(SignUpCommand command)
    {
        Result<object, FormattedError> result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }
    
    [HttpPost("sign-in")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn(SignInCommand command)
    {
        Result<string, FormattedError> result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}