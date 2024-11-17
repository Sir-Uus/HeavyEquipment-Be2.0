using Domain.Entities;
using MediatR;
using Persistence.Data;

namespace Application.Messages.Command.CreateMessage;

public class CreateMessageCommand
{
    public class Command : IRequest<bool>
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Message { get; set; }
    }

    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var chatMessage = new Message
            {
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId,
                Content = request.Message,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(chatMessage);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
