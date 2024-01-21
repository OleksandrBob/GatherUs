using GatherUs.API.Extensions;
using GatherUs.API.Handlers.Events;
using GatherUs.Core.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GatherUs.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : Controller
{
    private readonly IMediator _mediator;

    public EventController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [Authorize(Roles = AppConstants.OrganizerRole)]
    public async Task<IActionResult> CreateEvent([FromBody]CreateEventCommand command)
    {
        command.OrganizerId = User.GetLoggedInUserId();

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpGet("currentUser")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUserEvents([FromQuery]GetCurrentUserEventsQuery command)
    {
        command.OrganizerId = User.GetLoggedInUserId();

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }
}
