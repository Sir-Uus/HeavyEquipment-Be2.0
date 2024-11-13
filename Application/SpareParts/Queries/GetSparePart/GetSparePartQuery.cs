using Application.Core;
using Application.Dtos;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.SpareParts.Queries.GetSparePart
{
    public class GetSparePartQuery
    {
        public class Query : IRequest<Result<PaginatedList<SparePartVm>>>
        {
#nullable enable
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 6;
            public string? EquipmentName { get; set; }
            public string? PartName { get; set; }
            public string? PartNumber { get; set; }
            public string? Manufacturer { get; set; }
            public string? AvailabilityStatus { get; set; }
            public decimal? Price { get; set; }
            public decimal? MinPrice { get; set; }
            public decimal? MaxPrice { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PaginatedList<SparePartVm>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<PaginatedList<SparePartVm>>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var query = _context
                    .SpareParts.AsNoTracking()
                    .AsSplitQuery()
                    .Include(e => e.Equipment)
                    .Include(sf => sf.SparePartFeedbacks)
                    .Include(i => i.SparePartImage)
                    .OrderByDescending(x => x.Id)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.EquipmentName))
                {
                    var lowerCaseEquipmentName = request.EquipmentName.ToLower();
                    query = query.Where(x =>
                        x.Equipment.Name.ToLower().Contains(lowerCaseEquipmentName)
                    );
                }

                if (!string.IsNullOrEmpty(request.PartName))
                {
                    var lowerCasePartName = request.PartName.ToLower();
                    query = query.Where(x => x.PartName.ToLower().Contains(lowerCasePartName));
                }

                if (!string.IsNullOrEmpty(request.PartNumber))
                {
                    var lowerCasePartNumber = request.PartNumber.ToLower();
                    query = query.Where(x => x.PartNumber.ToLower().Contains(lowerCasePartNumber));
                }

                if (!string.IsNullOrEmpty(request.Manufacturer))
                {
                    var lowerCaseManufacturer = request.Manufacturer.ToLower();
                    query = query.Where(x =>
                        x.Manufacturer.ToLower().Contains(lowerCaseManufacturer)
                    );
                }

                if (!string.IsNullOrEmpty(request.AvailabilityStatus))
                {
                    var lowerCaseAvailabilityStatus = request.AvailabilityStatus.ToLower();
                    query = query.Where(x =>
                        x.AvailabilityStatus.ToLower().Contains(lowerCaseAvailabilityStatus)
                    );
                }

                if (request.Price.HasValue)
                {
                    var priceString = request.Price.Value.ToString();
                    query = query.Where(x => x.Price.ToString().Contains(priceString));
                }

                if (request.MinPrice.HasValue)
                {
                    query = query.Where(x => x.Price >= request.MinPrice.Value);
                }

                if (request.MaxPrice.HasValue)
                {
                    query = query.Where(x => x.Price <= request.MaxPrice.Value);
                }

                var totalItems = await query.CountAsync(cancellationToken);

                var sparePart = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var sparePartReturn = _mapper.Map<List<SparePartVm>>(sparePart);

                var paginatedList = new PaginatedList<SparePartVm>(
                    sparePartReturn,
                    totalItems,
                    request.PageNumber,
                    request.PageSize
                );

                return Result<PaginatedList<SparePartVm>>.Success(
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
