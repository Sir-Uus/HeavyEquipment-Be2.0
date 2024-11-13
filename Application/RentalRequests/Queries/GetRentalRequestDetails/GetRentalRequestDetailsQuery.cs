using Application.Core;
using Application.Dtos;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.RentalRequests.Queries.GetRentalRequestDetails
{
    public class GetRentalRequestDetailsQuery
    {
        public class Query : IRequest<Result<RentalRequestVm>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<RentalRequestVm>>
        {
            private readonly IMapper _mapper;
            private readonly DataContext _context;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<RentalRequestVm>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var rentalRequest = await _context
                    .RentalRequests.AsNoTracking()
                    .Include(x => x.User)
                    .Include(x => x.Equipment)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                var rentalRequestReturn = _mapper.Map<RentalRequestVm>(rentalRequest);

                return Result<RentalRequestVm>.Success(rentalRequestReturn);
            }
        }
    }
}
