using Application.Core;
using AutoMapper;
using MediatR;
using Persistence.Data;

namespace Application.TransactionDetails.Command.DeleteTransactionDetails;

public class DeleteTransactionDetailsCommand
{
    public class Command : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
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
            var TransactionDetail = await _context.TransactionDetails.FindAsync(request.Id);

            if (TransactionDetail == null)
                return null;

            TransactionDetail.IsDeleted = true;

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return Result<Unit>.Failure("Failed to delete transaksi detail");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
