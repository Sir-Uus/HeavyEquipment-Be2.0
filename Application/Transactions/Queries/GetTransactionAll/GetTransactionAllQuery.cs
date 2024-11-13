using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Transactions.Queries.GetTransactionAll;

public class GetTransactionAllQuery
{
    public class Query : IRequest<Result<List<TransactionVm>>> { }

    public class Handler : IRequestHandler<Query, Result<List<TransactionVm>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<List<TransactionVm>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var transaksi = await _context
                .Transactions.AsNoTracking()
                .AsSplitQuery()
                .ToListAsync(cancellationToken);

            var transaksiReturn = _mapper.Map<List<TransactionVm>>(transaksi);

            return Result<List<TransactionVm>>.Success(transaksiReturn);
        }
    }
}
