using System;
using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Payments.Queries.GetPaymentAll;

public class GetPaymentAllQuery
{
    public class Query : IRequest<Result<List<PaymentVm>>> { }

    public class Handler : IRequestHandler<Query, Result<List<PaymentVm>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<PaymentVm>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var totalItems = await _context.Payments.CountAsync(cancellationToken);

            var payments = await _context
                .Payments.AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.RentalRequest)
                .ToListAsync(cancellationToken);

            var paymentsReturn = _mapper.Map<List<PaymentVm>>(payments);

            return Result<List<PaymentVm>>.Success(paymentsReturn);
        }
    }
}
