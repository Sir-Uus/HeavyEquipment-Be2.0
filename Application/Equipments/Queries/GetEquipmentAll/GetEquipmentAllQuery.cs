using System;
using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Equipments.Queries.GetEquipmentAll;

public class GetEquipmentAllQuery
{
    public class Query : IRequest<Result<List<EquipmentVm>>> { }

    public class Handler : IRequestHandler<Query, Result<List<EquipmentVm>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<EquipmentVm>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var totalItems = await _context.Equipments.CountAsync(cancellationToken);

            var equipments = await _context
                .Equipments.AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.Images)
                .Include(x => x.PerformanceFeedbacks)
                .Include(x => x.MaintenancedRecords)
                .Include(x => x.RentalHistories)
                .Include(x => x.SpareParts)
                .ToListAsync(cancellationToken);

            var equipmentVmList = _mapper.Map<List<EquipmentVm>>(equipments);

            return Result<List<EquipmentVm>>.Success(equipmentVmList);
        }
    }
}
