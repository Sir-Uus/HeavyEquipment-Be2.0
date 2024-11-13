using Application.Core;
using Application.Dtos;
using Application.SetValidator;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.SpareParts.Command.CreateSparePart
{
    public class CreateSparePartCommand
    {
        public class Command : IRequest<Result<Unit>>
        {
            public SparePartsDto SparePartsDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.SparePartsDto).SetValidator(new SparePartValidator());
            }
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
                    x.Id == request.SparePartsDto.EquipmentId
                );

                if (equipment == null)
                {
                    return Result<Unit>.Failure("Equipment not found");
                }

                var sparePart = _mapper.Map<SparePart>(request.SparePartsDto);

                _context.SpareParts.Add(sparePart);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                    return Result<Unit>.Failure("Failed to create Sparepart");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
