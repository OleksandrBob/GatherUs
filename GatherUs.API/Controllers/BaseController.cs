using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GatherUs.API.Controllers;

public abstract class BaseController : Controller
{
    protected readonly IMediator _mediator;
    
    protected BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }
}