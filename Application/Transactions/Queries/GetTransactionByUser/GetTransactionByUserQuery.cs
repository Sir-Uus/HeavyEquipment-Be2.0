using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Transactions.Queries.GetTransactionByUser;

public class GetTransactionByUserQuery
{
    public class Query : IRequest<Result<PaginatedList<TransactionVm>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 6;
        public string UserId { get; set; } = default!;
    }

    public class Handler : IRequestHandler<Query, Result<PaginatedList<TransactionVm>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<PaginatedList<TransactionVm>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var query = _context
                .Transactions.AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.TransactionDetails)
                .Where(t => t.UserId == request.UserId)
                .OrderByDescending(x => x.TransactionDate)
                .AsQueryable();

            var totalItems = await query.CountAsync(cancellationToken);

            var transaksi = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var transaksiReturn = _mapper.Map<List<TransactionVm>>(transaksi);

            var paginatedList = new PaginatedList<TransactionVm>(
                transaksiReturn,
                totalItems,
                request.PageNumber,
                request.PageSize
            );

            return Result<PaginatedList<TransactionVm>>.Success(
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
