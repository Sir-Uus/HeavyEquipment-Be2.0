using Application.Core;
using Application.Dtos;
using Application.SetValidator;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Persistence.Data;

namespace Application.Equipments.Command.CreateEquipment
{
    public class CreateEquipmentCommand
    {
        public class Command : IRequest<Result<Unit>>
        {
            public EquipmentDto EquipmentDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.EquipmentDto).SetValidator(new EquipmentValidator());
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
                var entity = _mapper.Map<Equipment>(request.EquipmentDto);
                _context.Equipments.Add(entity);
                var result = await _context.SaveChangesAsync() > 0;
                if (!result)
                    return Result<Unit>.Failure("Failed to create Equipments");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
