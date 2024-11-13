using Application.Core;
using Application.Dtos;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.MaintenanceRecord.Queries.GetMaintenanceRecord
{
    public class GetMaintenanceRecordQuery
    {
        public class Query : IRequest<Result<PaginatedList<MaintenanceRecordVm>>>
        {
#nullable enable
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 6;
            public string? EquipmentName { get; set; }
            public string? MaintenanceDate { get; set; }
            public string? ServicedPerformed { get; set; }
            public string? ServiceProfider { get; set; }
            public decimal? Cost { get; set; }
            public string? NextMaintenanceDue { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PaginatedList<MaintenanceRecordVm>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<PaginatedList<MaintenanceRecordVm>>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var query = _context
                    .MaintenancedRecords.AsNoTracking()
                    .AsSplitQuery()
                    .Include(e => e.Equipment)
                    .OrderByDescending(x => x.Id)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.EquipmentName))
                {
                    var lowerCaseEquipmentName = request.EquipmentName.ToLower();
                    query = query.Where(x =>
                        x.Equipment.Name.ToLower().Contains(lowerCaseEquipmentName)
                    );
                }

                if (!string.IsNullOrWhiteSpace(request.MaintenanceDate))
                {
                    if (DateTime.TryParse(request.MaintenanceDate, out var targetDate))
                    {
                        targetDate = DateTime.SpecifyKind(targetDate, DateTimeKind.Utc);

                        var startDate = targetDate.Date;
                        var endDate = startDate.AddDays(1);

                        query = query.Where(f =>
                            f.MaintenanceDate >= startDate && f.MaintenanceDate < endDate
                        );
                    }
                }

                if (!string.IsNullOrEmpty(request.ServicedPerformed))
                {
                    var lowerCaseServicedPerformed = request.ServicedPerformed.ToLower();
                    query = query.Where(x =>
                        x.ServicedPerformed.ToLower().Contains(lowerCaseServicedPerformed)
                    );
                }

                if (!string.IsNullOrEmpty(request.ServiceProfider))
                {
                    var lowerCaseServiceProfider = request.ServiceProfider.ToLower();
                    query = query.Where(x =>
                        x.ServicedProvider.ToLower().Contains(lowerCaseServiceProfider)
                    );
                }

                if (!string.IsNullOrEmpty(request.ServicedPerformed))
                {
                    var lowerCaseServicedPerformed = request.ServicedPerformed.ToLower();
                    query = query.Where(x =>
                        x.ServicedPerformed.ToLower().Contains(lowerCaseServicedPerformed)
                    );
                }

                if (request.Cost.HasValue)
                {
                    var costString = request.Cost.Value.ToString();
                    query = query.Where(x => x.Cost.ToString().Contains(costString));
                }

                if (!string.IsNullOrWhiteSpace(request.NextMaintenanceDue))
                {
                    if (DateTime.TryParse(request.NextMaintenanceDue, out var targetDate))
                    {
                        targetDate = DateTime.SpecifyKind(targetDate, DateTimeKind.Utc);

                        var startDate = targetDate.Date;
                        var endDate = startDate.AddDays(1);

                        query = query.Where(f =>
                            f.NextMaintenanceDue >= startDate && f.NextMaintenanceDue < endDate
                        );
                    }
                }

                var totalItems = await query.CountAsync(cancellationToken);

                var maintenanceRecord = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var maintenanceReturn = _mapper.Map<List<MaintenanceRecordVm>>(maintenanceRecord);

                var paginatedList = new PaginatedList<MaintenanceRecordVm>(
                    maintenanceReturn,
                    totalItems,
                    request.PageNumber,
                    request.PageSize
                );

                return Result<PaginatedList<MaintenanceRecordVm>>.Success(
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
