using Application.Core;
using Application.Dtos;
using Application.SetValidator;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Persistence.Data;

namespace Application.TransactionDetails.Command.CreateTransactionDetail;

public class CreateTransactionDetailCommand
{
    public class Command : IRequest<Result<Unit>>
    {
        public TransactionDetailsDto TransactionDetailsDto { get; set; } = default!;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.TransactionDetailsDto).SetValidator(new TransactionDetailValidator());
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

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
#nullable enable
            var TransactionDetail = _mapper.Map<TransactionDetail>(request.TransactionDetailsDto);

            SparePart? sparePart = null;
            Equipment? equipment = null;

            if (request.TransactionDetailsDto.SparePartId.HasValue)
            {
                sparePart = await _context.SpareParts.FindAsync(
                    request.TransactionDetailsDto.SparePartId.Value
                );
            }

            if (request.TransactionDetailsDto.EquipmentId.HasValue)
            {
                equipment = await _context.Equipments.FindAsync(
                    request.TransactionDetailsDto.EquipmentId.Value
                );
            }

            if (sparePart == null && equipment == null)
            {
                return Result<Unit>.Failure("Neither spare part nor equipment was found.");
            }

            if (sparePart != null && sparePart.Stock < request.TransactionDetailsDto.Quantity)
            {
                return Result<Unit>.Failure("Insufficient spare part stock.");
            }

            if (equipment != null && equipment.Unit < request.TransactionDetailsDto.Quantity)
            {
                return Result<Unit>.Failure("Insufficient equipment units.");
            }

            if (sparePart != null)
            {
                sparePart.Stock -= request.TransactionDetailsDto.Quantity;
            }

            if (equipment != null)
            {
                equipment.Unit -= request.TransactionDetailsDto.Quantity;
            }

            _context.TransactionDetails.Add(TransactionDetail);

            var result = await _context.SaveChangesAsync() > 0;
            if (!result)
            {
                return Result<Unit>.Failure("Failed to create Transaksi Detail.");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
