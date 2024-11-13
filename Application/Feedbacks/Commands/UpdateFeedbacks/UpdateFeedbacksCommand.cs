using Application.Core;
using Application.Dtos;
using AutoMapper;
using MediatR;
using Persistence.Data;

namespace Application.Feedbacks.Commands.UpdateFeedbacks
{
    public class UpdateFeedbacksCommand
    {
        public class Command : IRequest<Result<Unit>>
        {
            public FeedbackDto FeedbackDto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var performanceFeedback = await _context.PerformanceFeedbacks.FindAsync(
                    request.FeedbackDto.Id
                );

                if (performanceFeedback == null)
                    return null;

                _mapper.Map(request.FeedbackDto, performanceFeedback);

                var result = await _context.SaveChangesAsync();

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
