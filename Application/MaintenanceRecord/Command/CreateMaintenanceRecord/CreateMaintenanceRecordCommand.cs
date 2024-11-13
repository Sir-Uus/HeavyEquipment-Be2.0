using Application.Core;
using Application.Dtos;
using Application.SetValidator;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.MaintenanceRecord.Command.CreateMaintenanceRecord
{
    public class CreateMaintenanceRecordCommand
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

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    RuleFor(x => x.MaintenanceRecordDto)
                        .SetValidator(new MaintenaceRecordValidator());
                }
            }

            public async Task<Result<Unit>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var equipment = await _context.Equipments.FirstOrDefaultAsync(x =>
                    x.Id == request.MaintenanceRecordDto.EquipmentId
                );

                if (equipment == null)
                {
                    return Result<Unit>.Failure("Equipment not found");
                }

                var maintenanceRecord = _mapper.Map<MaintenancedRecord>(
                    request.MaintenanceRecordDto
                );
                _context.MaintenancedRecords.Add(maintenanceRecord);
                var result = await _context.SaveChangesAsync() > 0;
                if (!result)
                    return Result<Unit>.Failure("Failed to create maintenance record");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
