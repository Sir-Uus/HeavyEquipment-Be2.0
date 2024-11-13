using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.RentalRequests.Queries.GetRentalRequest
{
    public class GetRentalRequestQuery
    {
        public class Query : IRequest<Result<PaginatedList<RentalRequestVm>>>
        {
#nullable enable
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public string? Status { get; set; }
            public string? PaymentStatus { get; set; }
            public string? UserName { get; set; }
            public string? EquipmentName { get; set; }
            public string? Invoice { get; set; }
            public string? StarDate { get; set; }
            public string? EndDate { get; set; }
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
                    .Include(x => x.User)
                    .Include(x => x.Equipment)
                    .Include(x => x.Payments)
                    .OrderByDescending(x => x.Id)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.Status))
                {
                    query = query.Where(x => x.Status == request.Status);
                }

                if (!string.IsNullOrEmpty(request.EquipmentName))
                {
                    var lowerCaseEquipment = request.EquipmentName.ToLower();
                    query = query.Where(x =>
                        x.Equipment.Name.ToLower().Contains(lowerCaseEquipment)
                    );
                }

                if (!string.IsNullOrEmpty(request.UserName))
                {
                    var lowerCaseUserName = request.UserName.ToLower();
                    query = query.Where(x =>
                        x.User.DisplayName.ToLower().Contains(lowerCaseUserName)
                    );
                }

                if (!string.IsNullOrEmpty(request.PaymentStatus))
                {
                    var lowerCaseUserName = request.PaymentStatus.ToLower();
                    query = query.Where(x =>
                        x.Payments.Any(p => p.PaymentStatus.ToLower().Contains(lowerCaseUserName))
                    );
                }

                if (!string.IsNullOrEmpty(request.Invoice))
                {
                    var lowerCaseInvoice = request.Invoice.ToLower();
                    query = query.Where(x => x.Invoice.ToLower().Contains(lowerCaseInvoice));
                }

                if (!string.IsNullOrWhiteSpace(request.StarDate))
                {
                    if (DateTime.TryParse(request.StarDate, out var targetDate))
                    {
                        targetDate = DateTime.SpecifyKind(targetDate, DateTimeKind.Utc);

                        var startDate = targetDate.Date;
                        var endDate = startDate.AddDays(1);

                        query = query.Where(f => f.StarDate >= startDate && f.StarDate < endDate);
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.EndDate))
                {
                    if (DateTime.TryParse(request.EndDate, out var targetDate))
                    {
                        targetDate = DateTime.SpecifyKind(targetDate, DateTimeKind.Utc);

                        var startDate = targetDate.Date;
                        var endDate = startDate.AddDays(1);

                        query = query.Where(f => f.EndDate >= startDate && f.EndDate < endDate);
                    }
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
}
