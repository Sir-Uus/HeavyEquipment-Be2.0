using Application.Core;
using Application.Dtos;
using AutoMapper;
using MediatR;
using Persistence.Data;

namespace Application.MaintenanceRecord.Command.UpdateMaintenanceRecord
{
    public class UpdateMaintenanceRecordCommand
    {
        public class Command : IRequest<Result<Unit>>
        {
            public MaintenanceRecordDto MaintenanceRecordDto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var maintenanceRecord = await _context.MaintenancedRecords.FindAsync(
                    request.MaintenanceRecordDto.Id
                );

                if (maintenanceRecord == null)
                    return null;

                _mapper.Map(request.MaintenanceRecordDto, maintenanceRecord);

                var result = await _context.SaveChangesAsync();

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
