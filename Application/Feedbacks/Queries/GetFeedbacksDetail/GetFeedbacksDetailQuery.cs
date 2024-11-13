using Application.Core;
using Application.Dtos;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Feedbacks.Queries.GetFeedbacksDetail
{
    public class GetFeedbacksDetailQuery
    {
        public class Query : IRequest<Result<FeedbackVm>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<FeedbackVm>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<FeedbackVm>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var feedbacks = await _context
                    .PerformanceFeedbacks.AsNoTracking()
                    .Include(e => e.Equipment)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);

                var feebackReturn = _mapper.Map<FeedbackVm>(feedbacks);

                return Result<FeedbackVm>.Success(feebackReturn);
            }
        }
    }
}
