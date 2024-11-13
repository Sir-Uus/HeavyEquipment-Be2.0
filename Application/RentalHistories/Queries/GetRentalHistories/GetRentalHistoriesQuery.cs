using Application.Core;
using Application.Dtos;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.RentalHistories.Queries.GetRentalHistories
{
    public class GetRentalHistoriesQuery
    {
        public class Query : IRequest<Result<PaginatedList<RentalHistoryVm>>>
        {
#nullable enable
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 6;
            public string? EquipmentName { get; set; }
            public string? Renter { get; set; }
            public string? Invoice { get; set; }
            public string? RentalStartDate { get; set; }
            public string? RentalEndDate { get; set; }
            public decimal? RentalCost { get; set; }
            public string? Location { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PaginatedList<RentalHistoryVm>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<PaginatedList<RentalHistoryVm>>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var query = _context
                    .RentalHistories.AsNoTracking()
                    .Include(e => e.Renter)
                    .Include(e => e.Equipment)
                    .OrderByDescending(e => e.Id)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.EquipmentName))
                {
                    var lowerCaseEquipmentName = request.EquipmentName.ToLower();
                    query = query.Where(x =>
                        x.Equipment.Name.ToLower().Contains(lowerCaseEquipmentName)
                    );
                }

                if (!string.IsNullOrEmpty(request.Renter))
                {
                    var lowerCaseRenter = request.Renter.ToLower();
                    query = query.Where(x =>
                        x.Renter.DisplayName.ToLower().Contains(lowerCaseRenter)
                    );
                }

                if (!string.IsNullOrEmpty(request.Invoice))
                {
                    var lowerCaseRenter = request.Invoice.ToLower();
                    query = query.Where(x => x.Invoice.ToLower().Contains(lowerCaseRenter));
                }

                if (!string.IsNullOrWhiteSpace(request.RentalStartDate))
                {
                    if (DateTime.TryParse(request.RentalStartDate, out var targetDate))
                    {
                        targetDate = DateTime.SpecifyKind(targetDate, DateTimeKind.Utc);

                        var startDate = targetDate.Date;
                        var endDate = startDate.AddDays(1);

                        query = query.Where(f =>
                            f.RentalStartDate >= startDate && f.RentalStartDate < endDate
                        );
                    }
                }

                if (request.RentalCost.HasValue)
                {
                    var rentalCostString = request.RentalCost.Value.ToString();
                    query = query.Where(x => x.RentalCost.ToString().Contains(rentalCostString));
                }

                if (!string.IsNullOrWhiteSpace(request.RentalEndDate))
                {
                    if (DateTime.TryParse(request.RentalEndDate, out var targetDate))
                    {
                        targetDate = DateTime.SpecifyKind(targetDate, DateTimeKind.Utc);

                        var startDate = targetDate.Date;
                        var endDate = startDate.AddDays(1);

                        query = query.Where(f =>
                            f.RentalEndDate >= startDate && f.RentalEndDate < endDate
                        );
                    }
                }

                if (!string.IsNullOrEmpty(request.Location))
                {
                    var lowerCaseLocation = request.Location.ToLower();
                    query = query.Where(x => x.Location.ToLower().Contains(lowerCaseLocation));
                }

                var totalItems = await query.CountAsync(cancellationToken);

                var rentalHistory = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var rentalHistoryReturn = _mapper.Map<List<RentalHistoryVm>>(rentalHistory);

                var paginatedList = new PaginatedList<RentalHistoryVm>(
                    rentalHistoryReturn,
                    totalItems,
                    request.PageNumber,
                    request.PageSize
                );

                return Result<PaginatedList<RentalHistoryVm>>.Success(
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
