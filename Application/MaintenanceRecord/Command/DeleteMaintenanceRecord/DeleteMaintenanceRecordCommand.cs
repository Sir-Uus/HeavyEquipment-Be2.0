using Application.Core;
using MediatR;
using Persistence.Data;

namespace Application.MaintenanceRecord.Command.DeleteMaintenanceRecord
{
    public class DeleteMaintenanceRecordCommand
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var maintenanceRecord = await _context.MaintenancedRecords.FindAsync(request.Id);

                if (maintenanceRecord == null)
                    return null;

                maintenanceRecord.IsDeleted = true;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                    return Result<Unit>.Failure("Failed to delete maintenance record");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
