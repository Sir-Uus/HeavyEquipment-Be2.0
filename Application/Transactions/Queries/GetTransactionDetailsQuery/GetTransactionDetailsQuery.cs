using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Transactions.Queries.GetTransactionDetailsQuery;

public class GetTransactionDetailsQuery
{
    public class Query : IRequest<Result<TransactionVm>>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<TransactionVm>>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<TransactionVm>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var transaksi = await _context
                .Transactions.AsNoTracking()
                .AsSplitQuery()
                .Include(t => t.TransactionDetails)
                .ThenInclude(td => td.Equipment)
                .Include(t => t.TransactionDetails)
                .ThenInclude(td => td.SparePart)
                .FirstOrDefaultAsync(t => t.Id == request.Id);

            var transaksiReturn = _mapper.Map<TransactionVm>(transaksi);

            return Result<TransactionVm>.Success(transaksiReturn);
        }
    }
}
