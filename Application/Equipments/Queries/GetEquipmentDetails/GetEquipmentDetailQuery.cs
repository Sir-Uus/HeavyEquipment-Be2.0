using Application.Core;
using Application.Dtos;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Equipments.Queries.GetEquipmentDetails
{
    public class GetEquipmentDetailQuery
    {
        public class Query : IRequest<Result<EquipmentVm>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<EquipmentVm>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<EquipmentVm>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var equipments = await _context
                    .Equipments.AsNoTracking()
                    .AsSplitQuery()
                    .Include(x => x.Images)
                    .Include(x => x.PerformanceFeedbacks)
                    .Include(x => x.MaintenancedRecords)
                    .Include(x => x.RentalHistories)
                    .Include(x => x.SpareParts)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);

                var equipmentReturn = _mapper.Map<EquipmentVm>(equipments);

                return Result<EquipmentVm>.Success(equipmentReturn);
            }
        }
    }
}
