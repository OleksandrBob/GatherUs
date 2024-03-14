using GatherUs.API.Extensions;
using GatherUs.API.Handlers.Users;
using GatherUs.Core.Constants;
using GatherUs.Core.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GatherUs.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : Controller
{
    private readonly IMediator _mediator;
    private readonly IPaymentService _paymentService;

    public UsersController(IMediator mediator, IPaymentService paymentService)
    {
        _mediator = mediator;
        _paymentService = paymentService;
    }

    [HttpGet]
    [Authorize(Roles = AppConstants.OrganizerRole)]
    public IActionResult ListProducts()
    {
        return Ok(1);
    }

    [HttpGet("current")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUserData()
    {
        GetUserDataQuery command = new()
        {
            UserId = User.GetLoggedInUserId(),
        };

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPut("current")]
    [Authorize]
    public async Task<IActionResult> UpdateCurrentUserData(UpdateCurrentUserDataCommand command)
    {
        command.UserId = User.GetLoggedInUserId();

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [HttpDelete("current/picture")]
    [Authorize]
    public async Task<IActionResult> RemoveUserPicture()
    {
        var command = new RemoveUserPictureCommand { CurrentUserId = User.GetLoggedInUserId() };

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [HttpPut("current/picture")]
    [Authorize]
    public async Task<IActionResult> UploadUserPicture([FromBody]UploadUserPictureCommand command)
    {
        command.CurrentUserId = User.GetLoggedInUserId();
        
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
    
    [HttpGet("current/payment-token")]
    [Authorize]
    public IActionResult GetPaymentToken()
    {
        var token = _paymentService.GenerateClientToken(User.GetLoggedInUserId());

        return Ok(token);
    }
    
    [HttpPost("current/process-payment")]
    [Authorize]
    public IActionResult ProcessPayment(string nonce)
    {
        var token = _paymentService.ProcessPayment(nonce);
        return Ok(token);
    }
}
