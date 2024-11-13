using Application.Core;
using Application.Dtos;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.MaintenanceRecord.Queries.GetMaintenanceRecordDetails
{
    public class GetMaintenanceRecordDetailsQuery
    {
        public class Query : IRequest<Result<MaintenanceRecordVm>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<MaintenanceRecordVm>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<MaintenanceRecordVm>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var maintenanceRecord = await _context
                    .MaintenancedRecords.AsNoTracking()
                    .Include(e => e.Equipment)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);

                var maintenanceReturn = _mapper.Map<MaintenanceRecordVm>(maintenanceRecord);

                return Result<MaintenanceRecordVm>.Success(maintenanceReturn);
            }
        }
    }
}
