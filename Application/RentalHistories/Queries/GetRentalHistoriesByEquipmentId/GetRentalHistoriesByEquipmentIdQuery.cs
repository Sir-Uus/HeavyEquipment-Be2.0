using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.RentalHistories.Queries.GetRentalHistoriesByEquipmentId;

public class GetRentalHistoriesByEquipmentIdQuery
{
    public class Query : IRequest<Result<List<RentalHistoryVm>>>
    {
        public int EquipmentId { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<RentalHistoryVm>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<List<RentalHistoryVm>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var rentalHistories = await _context
                .RentalHistories.AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.Equipment)
                .Where(rh => rh.EquipmentId == request.EquipmentId)
                .ToListAsync(cancellationToken);

            var rentalHistoriesReturn = _mapper.Map<List<RentalHistoryVm>>(rentalHistories);

            return Result<List<RentalHistoryVm>>.Success(rentalHistoriesReturn);
        }
    }
}
