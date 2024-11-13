using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Feedbacks.Queries.GetFeedbacksByEquipmentId;

public class GetFeedbacksByEquipmentIdQuery
{
    public class Query : IRequest<Result<List<FeedbackVm>>>
    {
        public int EquipmentId { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<FeedbackVm>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<List<FeedbackVm>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var feedbacks = await _context
                .PerformanceFeedbacks.AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.Equipment)
                .Where(pf => pf.EquipmentId == request.EquipmentId)
                .ToListAsync(cancellationToken);

            var feedbacksReturn = _mapper.Map<List<FeedbackVm>>(feedbacks);

            return Result<List<FeedbackVm>>.Success(feedbacksReturn);
        }
    }
}
