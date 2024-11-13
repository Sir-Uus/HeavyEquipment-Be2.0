using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.TransactionDetails.Queries.GetTransactionDetailDetails;

public class GetTransactionDetailDetailsQuery
{
    public class Query : IRequest<Result<TransactionDetailsVm>>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<TransactionDetailsVm>>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<TransactionDetailsVm>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var TransactionDetail = await _context
                .TransactionDetails.AsNoTracking()
                .Include(x => x.Transactions)
                .Include(x => x.Equipment)
                .Include(x => x.SparePart)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (TransactionDetail == null)
                return null;

            var TransactionDetailReturn = _mapper.Map<TransactionDetailsVm>(TransactionDetail);

            return Result<TransactionDetailsVm>.Success(TransactionDetailReturn);
        }
    }
}
