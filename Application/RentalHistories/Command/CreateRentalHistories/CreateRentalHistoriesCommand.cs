using Application.Core;
using Application.Dtos;
using Application.SetValidator;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.RentalHistories.Command.CreateRentalHistories
{
    public class CreateRentalHistoriesCommand
    {
        public class Command : IRequest<Result<Unit>>
        {
            public RentalHistoryDto RentalHistoryDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.RentalHistoryDto).SetValidator(new RentalHistoryValidator());
            }
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
                var equipment = await _context.Equipments.FirstOrDefaultAsync(x =>
                    x.Id == request.RentalHistoryDto.EquipmentId
                );

                var user = await _context.Users.FirstOrDefaultAsync(x =>
                    x.Id == request.RentalHistoryDto.RenterId
                );

                if (equipment == null)
                {
                    return Result<Unit>.Failure("Equipment not found");
                }

                if (user == null)
                {
                    return Result<Unit>.Failure("User must registered first");
                }

                var rentalHistory = _mapper.Map<RentalHistory>(request.RentalHistoryDto);

                _context.RentalHistories.Add(rentalHistory);

                var result = await _context.SaveChangesAsync() > 0;
                if (!result)
                    return Result<Unit>.Failure("Failed to create rental history");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
