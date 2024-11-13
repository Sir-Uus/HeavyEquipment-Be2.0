using System;
using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.SparePartFeedbacks.Queries.GetSparePartFeedback;

public class GetSparePartFeedbackQuery
{
    public class Query : IRequest<Result<PaginatedList<SparePartFeedbackVm>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 6;
    }

    public class Handler : IRequestHandler<Query, Result<PaginatedList<SparePartFeedbackVm>>>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PaginatedList<SparePartFeedbackVm>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var query = _context
                .SparePartFeedbacks.AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.User)
                .OrderByDescending(x => x.FeedbackDate)
                .AsQueryable();

            var totalItems = await query.CountAsync(cancellationToken);

            var sparePartFeedback = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var sparepartFeedbackReturn = _mapper.Map<List<SparePartFeedbackVm>>(sparePartFeedback);

            var paginatedList = new PaginatedList<SparePartFeedbackVm>(
                sparepartFeedbackReturn,
                totalItems,
                request.PageNumber,
                request.PageSize
            );

            return Result<PaginatedList<SparePartFeedbackVm>>.Success(
                paginatedList,
                totalCount: paginatedList.TotalCount,
                totalPages: paginatedList.TotalPages,
                currentPage: paginatedList.CurrentPage,
                pageSize: paginatedList.PageSize,
                hasPreviousPage: paginatedList.HasPreviousPage,
                hasNextPage: paginatedList.HasNextPage
            );
        }
    }
}
