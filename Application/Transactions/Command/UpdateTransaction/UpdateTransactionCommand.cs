using Application.Core;
using Application.Dtos;
using AutoMapper;
using MediatR;
using Persistence.Data;

namespace Application.Transactions.Command.UpdateTransaction;

public class UpdateTransactionCommand
{
    public class Command : IRequest<Result<Unit>>
    {
        public TransactionDto TransactionDto { get; set; } = default!;
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

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var transaksi = await _context.Transactions.FindAsync(request.TransactionDto.Id);

            if (transaksi == null)
                return null;

            _mapper.Map(request.TransactionDto, transaksi);

            var result = await _context.SaveChangesAsync();

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
