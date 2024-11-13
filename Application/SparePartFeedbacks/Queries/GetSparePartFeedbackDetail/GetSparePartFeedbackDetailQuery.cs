using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
// using Persistence.Migrations;

namespace Application.SparePartFeedbacks.Queries.GetSparePartFeedbackDetail;

public class GetSparePartFeedbackDetailQuery
{
    public class Query : IRequest<Result<SparePartFeedbackVm>>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<SparePartFeedbackVm>>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<SparePartFeedbackVm>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var sparePartFeedback = await _context
                .SparePartFeedbacks.AsNoTracking()
                .Include(x => x.SparePart)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (sparePartFeedback == null)
                return null;

            var sparePartFeedbackReturn = _mapper.Map<SparePartFeedbackVm>(sparePartFeedback);

            return Result<SparePartFeedbackVm>.Success(sparePartFeedbackReturn);
        }
    }
}
