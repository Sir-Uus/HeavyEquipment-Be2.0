using Application.Core;
using MediatR;
using Persistence.Data;

namespace Application.Transactions.Command.DeleteTransaction;

public class DeleteTransactionCommand
{
    public class Command : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var transaksi = await _context.Transactions.FindAsync(request.Id);

            if (transaksi == null)
                return null;

            transaksi.IsDeleted = true;

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
                return Result<Unit>.Failure("Failed to delete Transaksi");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
