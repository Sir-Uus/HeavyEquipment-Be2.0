using Application.Core;
using Application.Dtos;
using Application.SetValidator;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Equipments.Command.UpdateEquipment
{
    public class UpdateEquipmentCommand
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
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var equipment = await _context.Equipments.FindAsync(request.EquipmentDto.Id);

                if (equipment == null)
                    return Result<Unit>.Failure("Equipment not found");

                _mapper.Map(request.EquipmentDto, equipment);

                if (request.EquipmentDto.Images != null)
                {
                    var existingImage = await _context.Images.FirstOrDefaultAsync(
                        img => img.EquipmentId == equipment.Id,
                        cancellationToken
                    );

                    if (existingImage != null)
                    {
                        existingImage.ContenType = request.EquipmentDto.Images.ContenType;
                        existingImage.FileName = request.EquipmentDto.Images.FileName;
                        existingImage.Image = request.EquipmentDto.Images.Image;
                    }
                    else
                    {
                        var newImage = new Images
                        {
                            EquipmentId = equipment.Id,
                            ContenType = request.EquipmentDto.Images.ContenType,
                            FileName = request.EquipmentDto.Images.FileName,
                            Image = request.EquipmentDto.Images.Image,
                            UploadedAt = DateTime.UtcNow
                        };
                        await _context.Images.AddAsync(newImage, cancellationToken);
                    }
                }

                if (equipment.Unit > 0)
                {
                    equipment.Status = "Available";
                }
                else
                {
                    equipment.Status = "Unavailable";
                }

                await _context.SaveChangesAsync(cancellationToken);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
