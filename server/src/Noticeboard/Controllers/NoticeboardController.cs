using MediatR;
using Microsoft.AspNetCore.Mvc;
using Noticeboard.Core.Handlers;
using Noticeboard.Core.Models;

namespace Noticeboard.Core.Controllers;

[ApiController]
[Route("/noticeboard")]
public class NoticeboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public NoticeboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("post")]
    public async Task<ActionResult> Post([FromForm]NoticeRequest content)
    {
        var query = new NoticeboardHandler.Query(content);
        var res = await _mediator.Send(query);
        
        return res.Match<ActionResult>(
            Ok,
            _ => new BadRequestResult()
        );
    }
}