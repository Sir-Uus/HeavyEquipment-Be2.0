using Application.Core;
using Application.Dtos;
using Application.HubGathering.Stockhub;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.SpareParts.Command.UpdateSparePart
{
    public class UpdateSparePartCommand
    {
        public class Command : IRequest<Result<Unit>>
        {
            public SparePartsDto SparePartsDto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IMapper _mapper;
            private readonly DataContext _context;
            private readonly IHubContext<StockHub> _stockHubContext;

            public Handler(
                DataContext context,
                IMapper mapper,
                IHubContext<StockHub> stockHubContext
            )
            {
                _stockHubContext = stockHubContext;
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
                var sparePart = await _context.SpareParts.FindAsync(request.SparePartsDto.Id);

                if (request.SparePartsDto.SparePartImage != null)
                {
                    var existingImage = await _context.sparePartImages.FirstOrDefaultAsync(
                        img => img.SparePartId == sparePart.Id,
                        cancellationToken
                    );

                    if (existingImage != null)
                    {
                        existingImage.ContenType = request.SparePartsDto.SparePartImage.ContenType;
                        existingImage.FileName = request.SparePartsDto.SparePartImage.FileName;
                        existingImage.Image = request.SparePartsDto.SparePartImage.Image;
                    }
                    else
                    {
                        var newImage = new SparePartImage
                        {
                            SparePartId = sparePart.Id,
                            ContenType = request.SparePartsDto.SparePartImage.ContenType,
                            FileName = request.SparePartsDto.SparePartImage.FileName,
                            Image = request.SparePartsDto.SparePartImage.Image,
                            UploadedAt = DateTime.UtcNow
                        };
                        await _context.sparePartImages.AddAsync(newImage, cancellationToken);
                    }
                }

                if (sparePart == null)
                {
                    return Result<Unit>.Failure("Spare Part not found");
                }

                _mapper.Map(request.SparePartsDto, sparePart);

                if (sparePart.Stock == 0)
                {
                    sparePart.AvailabilityStatus = "Out of Stock";
                }
                else
                {
                    sparePart.AvailabilityStatus = "In Stock";
                }

                if (sparePart.Stock > 0)
                {
                    sparePart.AvailabilityStatus = "In Stock";
                }
                else
                {
                    sparePart.AvailabilityStatus = "Out of Stock";
                }

                await _context.SaveChangesAsync();
                ;
                await _stockHubContext.Clients.All.SendAsync(
                    "ReceiveStockUpdate",
                    sparePart.Id,
                    sparePart.Stock
                );

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
