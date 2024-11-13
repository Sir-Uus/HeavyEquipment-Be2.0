using Application.Core;
using Application.Dtos;
using Application.SetValidator;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Persistence.Data;

namespace Application.Transactions.Command.CreateTransaction;

public class CreateTransactionCommand
{
    public class Command : IRequest<Result<TransactionDto>>
    {
        public TransactionDto TransactionDto { get; set; } = default!;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.TransactionDto).SetValidator(new TransaksiValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<TransactionDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<TransactionDto>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var transaksi = _mapper.Map<Transaction>(request.TransactionDto);

            _context.Transactions.Add(transaksi);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return Result<TransactionDto>.Failure("Failed to create Transaksi");

            var transaksiReturn = _mapper.Map<TransactionDto>(transaksi);

            return Result<TransactionDto>.Success(transaksiReturn);
        }
    }
}
