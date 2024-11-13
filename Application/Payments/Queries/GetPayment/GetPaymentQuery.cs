using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Payments.Queries.GetPayment
{
    public class GetPaymentQuery
    {
        public class Query : IRequest<Result<PaginatedList<PaymentVm>>>
        {
#nullable enable
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 6;
            public string? Invoice { get; set; }
            public decimal? Amount { get; set; }
            public string? PaymentMethod { get; set; }
            public string? PaymentStatus { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PaginatedList<PaymentVm>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<PaginatedList<PaymentVm>>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var query = _context
                    .Payments.AsNoTracking()
                    .AsSplitQuery()
                    .Include(x => x.RentalRequest)
                    .OrderByDescending(x => x.PaymentDate)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.Invoice))
                {
                    var lowerCaseInvoice = request.Invoice.ToLower();
                    query = query.Where(x =>
                        x.RentalRequest.Invoice.ToLower().Contains(lowerCaseInvoice)
                    );
                }

                if (request.Amount.HasValue)
                {
                    var amountString = request.Amount.Value.ToString();
                    query = query.Where(x => x.Amount.ToString().Contains(amountString));
                }

                if (!string.IsNullOrEmpty(request.PaymentStatus))
                {
                    var lowerCasePaymentStatus = request.PaymentStatus.ToLower();
                    query = query.Where(x =>
                        x.PaymentStatus.ToLower().Contains(lowerCasePaymentStatus)
                    );
                }

                if (!string.IsNullOrEmpty(request.PaymentMethod))
                {
                    var lowerCasePaymentMethod = request.PaymentMethod.ToLower();
                    query = query.Where(x =>
                        x.PaymentMethod.ToLower().Contains(lowerCasePaymentMethod)
                    );
                }

                var totalItems = await query.CountAsync(cancellationToken);

                var payments = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var paymentsReturn = _mapper.Map<List<PaymentVm>>(payments);

                var paginatedList = new PaginatedList<PaymentVm>(
                    paymentsReturn,
                    totalItems,
                    request.PageNumber,
                    request.PageSize
                );

                return Result<PaginatedList<PaymentVm>>.Success(
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
}
