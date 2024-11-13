using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Messages.Query.GetMessage
{
    public class GetMessageQuery
    {
        public class Query : IRequest<List<Message>>
        {
            public string UserId { get; set; }
            public string OtherUserId { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Message>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<Message>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                return await _context
                    .Messages.Where(m =>
                        (m.SenderId == request.UserId && m.ReceiverId == request.OtherUserId)
                        || (m.SenderId == request.OtherUserId && m.ReceiverId == request.UserId)
                    )
                    .OrderBy(m => m.SentAt)
                    .ToListAsync();
            }
        }
    }
}
