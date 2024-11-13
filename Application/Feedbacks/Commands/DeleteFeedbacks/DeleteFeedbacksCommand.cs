using Application.Core;
using MediatR;
using Persistence.Data;

namespace Application.Feedbacks.Commands.DeleteFeedbacks
{
    public class DeleteFeedbacksCommand
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var performanceFeedbacks = await _context.PerformanceFeedbacks.FindAsync(
                    request.Id
                );

                if (performanceFeedbacks == null)
                    return null;

                performanceFeedbacks.IsDeleted = true;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                    return Result<Unit>.Failure("Failed to deleting feedbacks");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
