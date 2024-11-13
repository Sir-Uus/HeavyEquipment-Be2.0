using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.TransactionDetails.Queries.GetTransactionDetails;

public class GetTransactionDetailsQuery
{
    public class Query : IRequest<Result<PaginatedList<TransactionDetailsVm>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 6;
    }

    public class Handler : IRequestHandler<Query, Result<PaginatedList<TransactionDetailsVm>>>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PaginatedList<TransactionDetailsVm>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var query = _context
                .TransactionDetails.AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.Transactions)
                .Include(x => x.Equipment)
                .Include(x => x.SparePart)
                .OrderByDescending(x => x.Id)
                .AsQueryable();

            var totalItems = await query.CountAsync(cancellationToken);

            var transactionDetail = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var transactionDetailReturn = _mapper.Map<List<TransactionDetailsVm>>(
                transactionDetail
            );

            var paginatedList = new PaginatedList<TransactionDetailsVm>(
                transactionDetailReturn,
                totalItems,
                request.PageSize,
                request.PageNumber
            );

            return Result<PaginatedList<TransactionDetailsVm>>.Success(paginatedList);
        }
    }
}
