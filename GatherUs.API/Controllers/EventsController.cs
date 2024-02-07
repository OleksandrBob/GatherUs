using GatherUs.API.Extensions;
using GatherUs.API.Handlers.Events;
using GatherUs.Core.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GatherUs.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController : Controller
{
    private readonly IMediator _mediator;

    public EventsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetEvents([FromQuery] SearchEventQuery command)
    {
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpGet("currentUser")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUserEvents([FromQuery] GetCurrentUserEventsQuery command)
    {
        command.OrganizerId = User.GetLoggedInUserId();

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpPost("{id}/attend")]
    [Authorize(Roles = AppConstants.OrganizerRole)]
    public async Task<IActionResult> AttendEvent([FromRoute] int id)
    {
        AttendEventCommand command =
            new()
            {
                GuestId = User.GetLoggedInUserId(),
                EventId = id,
            };

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result.Error);
    }

    [HttpPost]
    [Authorize(Roles = AppConstants.OrganizerRole)]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventCommand command)
    {
        command.OrganizerId = User.GetLoggedInUserId();

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpPost("invites")]
    [Authorize(Roles = AppConstants.OrganizerRole)]
    public async Task<IActionResult> InviteUserToEvent([FromBody] InviteUserToEventCommand command)
    {
        command.OrganizerId = User.GetLoggedInUserId();

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(result.Error);
    }

    [HttpPost("invites/{id}")]
    [Authorize(Roles = AppConstants.GuestRole)]
    public async Task<IActionResult> SetInviteStatus([FromRoute] int id, [FromBody] SetInviteStatusCommand command)
    {
        command.InviteId = id;

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(result.Error);
    }

    [HttpGet("{id}/invites")]
    [Authorize(Roles = AppConstants.OrganizerRole)]
    public async Task<IActionResult> GetInvitedGuests([FromRoute] int id)
    {
        var command = new GetInvitedGuestsToEventQuery
        {
            OrganizerId = User.GetLoggedInUserId(),
            CustomEventId = id,
        };

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpGet("invites")]
    [Authorize(Roles = AppConstants.GuestRole)]
    public async Task<IActionResult> GetGuestInvites([FromQuery] GetGuestInvitesQuery command)
    {
        command.GuestId = User.GetLoggedInUserId();

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }
}
