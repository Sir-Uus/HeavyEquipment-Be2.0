using System;
using Application.Core;
using Application.Vm;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.SparePartFeedbacks.Queries.GetSparePartFeedbackBySparePart;

public class GetSparePartFeedbackBySparePartQuery
{
    public class Query : IRequest<Result<List<SparePartFeedbackVm>>>
    {
        public int SparePartId { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<SparePartFeedbackVm>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<List<SparePartFeedbackVm>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var sparePartFeedback = await _context
                .SparePartFeedbacks.AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.User)
                .Where(x => x.SparePartId == request.SparePartId)
                .ToListAsync(cancellationToken);

            var sparePartFeedbackReturn = _mapper.Map<List<SparePartFeedbackVm>>(sparePartFeedback);

            return Result<List<SparePartFeedbackVm>>.Success(sparePartFeedbackReturn);
        }
    }
}
