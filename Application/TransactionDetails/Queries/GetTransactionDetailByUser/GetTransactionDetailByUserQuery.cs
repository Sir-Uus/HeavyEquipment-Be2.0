using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.TransactionDetails.Queries.GetTransactionDetailByUser;

public class GetTransactionDetailByUserQuery
{
    public class Query : IRequest<Result<PaginatedList<TransactionDetailsVm>>>
    {
#nullable enable
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 6;
        public string? UserId { get; set; }
        public string? Status { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<PaginatedList<TransactionDetailsVm>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
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
                .Where(x => x.Transactions.UserId == request.UserId)
                .Where(x => x.EquipmentId == null)
                .OrderByDescending(x => x.Id)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Status))
            {
                var lowerCaseStatus = request.Status.ToLower();
                query = query.Where(x => x.Transactions.Status.ToLower().Contains(lowerCaseStatus));
            }

            var totalItems = await query.CountAsync(cancellationToken);

            var transactionDetails = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var transactionDetailsVms = _mapper.Map<List<TransactionDetailsVm>>(transactionDetails);

            var paginatedList = new PaginatedList<TransactionDetailsVm>(
                transactionDetailsVms,
                totalItems,
                request.PageNumber,
                request.PageSize
            );

            return Result<PaginatedList<TransactionDetailsVm>>.Success(
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
