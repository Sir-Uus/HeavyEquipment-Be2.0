using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Equipments.Queries.GetEquipments
{
    public class GetEquipmentQuery
    {
        public class Query : IRequest<Result<PaginatedList<EquipmentVm>>>
        {
#nullable enable
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 6;
            public string? SearchTerm { get; set; }
            public string? Type { get; set; }
            public string? Status { get; set; }
            public string? Location { get; set; }
            public string? Brand { get; set; }
            public string? Model { get; set; }
            public string? Specification { get; set; }
            public string? Description { get; set; }
            public string? YearOfManufacture { get; set; }
            public decimal? RentalPrice { get; set; }
            public int? Unit { get; set; }
            public decimal? MinPrice { get; set; }
            public decimal? MaxPrice { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PaginatedList<EquipmentVm>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<PaginatedList<EquipmentVm>>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var query = _context
                    .Equipments.AsNoTracking()
                    .AsSplitQuery()
                    .Include(x => x.PerformanceFeedbacks)
                    .Include(x => x.Images)
                    .OrderByDescending(x => x.Id)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    var lowerCaseSearchTerm = request.SearchTerm.ToLower();
                    query = query.Where(x => x.Name.ToLower().Contains(lowerCaseSearchTerm));
                }

                if (!string.IsNullOrEmpty(request.Type))
                {
                    query = query.Where(x => x.Type == request.Type);
                }

                if (!string.IsNullOrEmpty(request.Status))
                {
                    query = query.Where(x => x.Status == request.Status);
                }

                if (!string.IsNullOrEmpty(request.Location))
                {
                    var lowerCaseLocation = request.Location.ToLower();
                    query = query.Where(x => x.Location.ToLower().Contains(lowerCaseLocation));
                }

                if (!string.IsNullOrEmpty(request.Brand))
                {
                    var lowerCaseBrand = request.Brand.ToLower();
                    query = query.Where(x => x.Brand.ToLower().Contains(lowerCaseBrand));
                }

                if (!string.IsNullOrEmpty(request.Model))
                {
                    var lowerCaseModel = request.Model.ToLower();
                    query = query.Where(x => x.Model.ToLower().Contains(lowerCaseModel));
                }

                if (!string.IsNullOrEmpty(request.Specification))
                {
                    var lowerCaseSpecification = request.Specification.ToLower();
                    query = query.Where(x =>
                        x.Specification.ToLower().Contains(lowerCaseSpecification)
                    );
                }

                if (!string.IsNullOrEmpty(request.Description))
                {
                    var lowerCaseDescription = request.Description.ToLower();
                    query = query.Where(x =>
                        x.Description.ToLower().Contains(lowerCaseDescription)
                    );
                }

                if (!string.IsNullOrEmpty(request.YearOfManufacture))
                {
                    var lowerCaseYearOfManufcture = request.YearOfManufacture.ToLower();
                    query = query.Where(x =>
                        x.YearOfManufacture.ToLower().Contains(lowerCaseYearOfManufcture)
                    );
                }

                if (request.RentalPrice.HasValue)
                {
                    var rentalPriceString = request.RentalPrice.Value.ToString();
                    query = query.Where(x => x.RentalPrice.ToString().Contains(rentalPriceString));
                }

                if (request.Unit.HasValue)
                {
                    query = query.Where(x => x.Unit == request.Unit.Value);
                }

                if (request.MinPrice.HasValue)
                {
                    query = query.Where(x => x.RentalPrice >= request.MinPrice.Value);
                }

                if (request.MaxPrice.HasValue)
                {
                    query = query.Where(x => x.RentalPrice <= request.MaxPrice.Value);
                }

                var totalItems = await query.CountAsync(cancellationToken);

                var equipments = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var equipmentVmList = _mapper.Map<List<EquipmentVm>>(equipments);

                var paginatedList = new PaginatedList<EquipmentVm>(
                    equipmentVmList,
                    totalItems,
                    request.PageNumber,
                    request.PageSize
                );

                return Result<PaginatedList<EquipmentVm>>.Success(
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
