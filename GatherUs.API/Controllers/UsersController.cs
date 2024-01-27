using GatherUs.Core.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GatherUs.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : Controller
{
    private readonly IMediator _mediator;
    
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Authorize(Roles = AppConstants.OrganizerRole)]
    public IActionResult ListProducts()
    {
        return Ok(1);
    }

    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetUser([FromRoute]int id)
    {
        return BadRequest(id);
    }
}
