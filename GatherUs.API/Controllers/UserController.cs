using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GatherUs.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly IMediator _mediator;
    
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public IActionResult ListProducts()
    {
        return Ok(1);
    }

    [HttpGet("{id}")]
    public IActionResult GetUser([FromRoute]int id)
    {

        return BadRequest(id);
    }
}