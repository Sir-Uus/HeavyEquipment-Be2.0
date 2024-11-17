using Application.Messages.Command.CreateMessage;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Persistence.Data;

namespace Application.Messages.Command.ChatHub;

public class ChatHubClass : Hub
{
    private readonly IMediator _mediator;
    private readonly DataContext _context;

    public ChatHubClass(DataContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public override Task OnConnectedAsync()
    {
        var userId = Context
            .User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
            ?.Value;
        Console.WriteLine($"User connected: {userId}");
        return base.OnConnectedAsync();
    }

    public async Task SendMessage(string senderId, string receiverId, string message)
    {
        var command = new CreateMessageCommand.Command
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Message = message
        };

        var result = await _mediator.Send(command);

        if (result)
        {
            Console.WriteLine($"Message from {senderId} to {receiverId}: {message}");
            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);
        }
    }
}
