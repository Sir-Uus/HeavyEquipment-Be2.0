using Application.Core;
using Application.Dtos;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.RentalHistories.Queries.GetRentalHistoriesDetails
{
    public class GetRentalHistoriesDetailsQuery
    {
        public class Query : IRequest<Result<RentalHistoryVm>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<RentalHistoryVm>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<RentalHistoryVm>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var rentalHistory = await _context
                    .RentalHistories.AsNoTracking()
                    .AsSplitQuery()
                    .Include(e => e.Equipment)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);

                var rentalHistoryReturn = _mapper.Map<RentalHistoryVm>(rentalHistory);

                return Result<RentalHistoryVm>.Success(rentalHistoryReturn);
            }
        }
    }
}
