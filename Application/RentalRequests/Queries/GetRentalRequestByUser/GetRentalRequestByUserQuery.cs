using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.RentalRequests.Queries.GetRentalRequestByUser;

public class GetRentalRequestByUserQuery
{
    public class Query : IRequest<Result<PaginatedList<RentalRequestVm>>>
    {
#nullable enable
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 6;
        public string UserId { get; set; } = default!;
        public string? PaymentStatus { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<PaginatedList<RentalRequestVm>>>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PaginatedList<RentalRequestVm>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var query = _context
                .RentalRequests.AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.Equipment)
                .Include(x => x.Payments)
                .Include(x => x.User)
                .Where(u => u.UserId == request.UserId)
                .OrderByDescending(x => x.Id)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.PaymentStatus))
            {
                var lowerCaseUserName = request.PaymentStatus.ToLower();
                query = query.Where(x =>
                    x.Payments.Any(p => p.PaymentStatus.ToLower().Contains(lowerCaseUserName))
                );
            }

            var totalItems = await query.CountAsync(cancellationToken);

            var rentalRequest = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var rentalRequestReturn = _mapper.Map<List<RentalRequestVm>>(rentalRequest);

            var paginatedList = new PaginatedList<RentalRequestVm>(
                rentalRequestReturn,
                totalItems,
                request.PageNumber,
                request.PageSize
            );

            return Result<PaginatedList<RentalRequestVm>>.Success(
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
