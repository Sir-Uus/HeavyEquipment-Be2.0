using Application.Messages.Query.GetMessage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MessageController : BaseApiController
{
    private readonly IMediator _mediator;

    public MessageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetChatHistory(string userId, string otherUserId)
    {
        var query = new GetMessageQuery.Query { UserId = userId, OtherUserId = otherUserId };
        var messages = await _mediator.Send(query);
        return Ok(messages);
    }
}
