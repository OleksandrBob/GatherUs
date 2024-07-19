using GatherUs.API.Handlers.Accounts;
using GatherUs.Core.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GatherUs.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : BaseController
{
    public AccountsController(IMediator mediator) : base(mediator) { }

    [HttpPost("get-app-info")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAppInfo()
    {
        var aaa = EnvironmentVariablesHelper.SmtpHost;
        return Ok(aaa);
    }

    [HttpPost("get-confirmation-code")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRegistrationConfirmationCode(GetRegistrationConfirmationCodeCommand command)
    {
        var result = await _mediator.Send(command);

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
        var result = await _mediator.Send(command);

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
        var result = await _mediator.Send(command);
        
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}