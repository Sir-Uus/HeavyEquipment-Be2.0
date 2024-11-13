using System;
using Application.Core;
using Application.Dtos;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.RentalRequests.Queries.GetRentalRequestAll;

public class GetRentalRequestAllQuery
{
    public class Query : IRequest<Result<List<RentalRequestVm>>> { }

    public class Handler : IRequestHandler<Query, Result<List<RentalRequestVm>>>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<RentalRequestVm>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var totalItems = await _context.RentalRequests.CountAsync(cancellationToken);

            var rentalRequest = await _context
                .RentalRequests.AsNoTracking()
                .AsSplitQuery()
                .Include(p => p.Payments)
                .ToListAsync(cancellationToken);

            var rentalRequestReturn = _mapper.Map<List<RentalRequestVm>>(rentalRequest);

            return Result<List<RentalRequestVm>>.Success(rentalRequestReturn);
        }
    }
}
