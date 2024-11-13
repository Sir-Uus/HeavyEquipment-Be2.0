using Application.Core;
using Application.Dtos;
using Application.SetValidator;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Persistence.Data;

namespace Application.RentalRequests.Command.CreateRentalRequest
{
    public class CreateRentalRequestCommand
    {
        public class Command : IRequest<Result<RentalRequestDto>>
        {
            public RentalRequestDto RentalRequestDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.RentalRequestDto).SetValidator(new RentalRequestValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<RentalRequestDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<RentalRequestDto>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var rentalRequest = _mapper.Map<RentalRequest>(request.RentalRequestDto);

                _context.RentalRequests.Add(rentalRequest);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                    return Result<RentalRequestDto>.Failure("Failed to create rental request");

                var rentalRequestDto = _mapper.Map<RentalRequestDto>(rentalRequest);

                return Result<RentalRequestDto>.Success(rentalRequestDto);
            }
        }
    }
}
