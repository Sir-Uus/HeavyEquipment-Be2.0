using Application.Core;
using Application.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.RentalHistories.Command.UpdateRentalHistories
{
    public class UpdateRentalHistoriesCommand
    {
        public class Command : IRequest<Result<Unit>>
        {
            public RentalHistoryDto RentalHistoryDto { get; set; }
            public object MaintenanceRecordDto { get; internal set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IMapper _mapper;
            private readonly DataContext _context;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var equipment = await _context.Equipments.FirstOrDefaultAsync(x =>
                    x.Id == request.RentalHistoryDto.EquipmentId
                );

                if (equipment == null)
                {
                    return Result<Unit>.Failure("Equipment not found");
                }

                var rentalHistory = await _context.RentalHistories.FindAsync(
                    request.RentalHistoryDto.Id
                );

                if (rentalHistory == null)
                    return null;

                _mapper.Map(request.RentalHistoryDto, rentalHistory);

                var result = await _context.SaveChangesAsync();

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
