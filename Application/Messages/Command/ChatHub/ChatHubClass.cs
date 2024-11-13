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

    public async Task SendMessageToAll(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendMessageToReceiver(string sender, string receiver, string message)
    {
        var userId = _context.Users.FirstOrDefault(u => u.Email.ToLower() == receiver.ToLower()).Id;

        if (!string.IsNullOrEmpty(userId))
        {
            Console.WriteLine("Sending message to user: " + userId);
            await Clients.User(userId).SendAsync("ReceiveMessage", sender, message);
        }
    }

    // public async Task SendMessage(string senderId, string receiverId, string message)
    // {
    //     if (string.IsNullOrEmpty(senderId))
    //     {
    //         throw new HubException("Sender ID is missing");
    //     }

    //     // await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, displayName, message);

    //     await Clients.All.SendAsync("ReceiveMessage", senderId, message);

    //     var command = new CreateMessageCommand.Command
    //     {
    //         SenderId = senderId,
    //         ReceiverId = receiverId,
    //         Message = message
    //     };

    //     await _mediator.Send(command);
    //     Console.WriteLine($"Received message from {senderId} to {receiverId}: {message}");
    // }
}
