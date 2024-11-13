using System;
using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Payments.Queries.GetPaymentByRentalRequestId;

public class GetPaymentByRentalRequestIdQuery
{
    public class Query : IRequest<Result<PaymentVm>>
    {
        public int RentalRequestId { get; set; }
    }

    public class Command : IRequestHandler<Query, Result<PaymentVm>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Command(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PaymentVm>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var payments = await _context
                .Payments.AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.RentalRequest)
                .FirstOrDefaultAsync(x => x.RentalRequestId == request.RentalRequestId);

            if (payments == null)
                return null;

            var paymentsReturn = _mapper.Map<PaymentVm>(payments);

            return Result<PaymentVm>.Success(paymentsReturn);
        }
    }
}
