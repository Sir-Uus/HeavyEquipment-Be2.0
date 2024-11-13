using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Transactions.Queries.GetTransactionQuery;

public class GetTransactionQuery
{
    public class Query : IRequest<Result<PaginatedList<TransactionVm>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 6;
    }

    public class Handler : IRequestHandler<Query, Result<PaginatedList<TransactionVm>>>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PaginatedList<TransactionVm>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var totalItems = await _context.Transactions.CountAsync(cancellationToken);

            var transaksi = await _context
                .Transactions.AsNoTracking()
                .AsSplitQuery()
                .OrderByDescending(x => x.TransactionDate)
                .Include(x => x.TransactionDetails)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var transaksiReturn = _mapper.Map<List<TransactionVm>>(transaksi);

            var paginatedList = new PaginatedList<TransactionVm>(
                transaksiReturn,
                totalItems,
                request.PageSize,
                request.PageNumber
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
