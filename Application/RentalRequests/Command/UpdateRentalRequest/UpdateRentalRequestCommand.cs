using Application.Core;
using Application.Dtos;
using AutoMapper;
using MediatR;
using Persistence.Data;

namespace Application.RentalRequests.Command.UpdateRentalRequest
{
    public class UpdateRentalRequestCommand
    {
        public class Command : IRequest<Result<Unit>>
        {
            public RentalRequestDto RentalRequestDto { get; set; }
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
                var rentalRequest = await _context.RentalRequests.FindAsync(
                    request.RentalRequestDto.Id
                );

                if (rentalRequest == null)
                    return Result<Unit>.Failure("Rental request not found");

                var equipment = await _context.Equipments.FindAsync(rentalRequest.EquipmentId);
                if (equipment == null)
                    return Result<Unit>.Failure("Associated equipment not found");

                if (request.RentalRequestDto.Status == "Approved")
                {
                    if (equipment.Unit <= 0)
                    {
                        return Result<Unit>.Failure(
                            "Cannot approve rental request: No available units for the selected equipment."
                        );
                    }
                }

                _mapper.Map(request.RentalRequestDto, rentalRequest);

                var result = await _context.SaveChangesAsync();

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
