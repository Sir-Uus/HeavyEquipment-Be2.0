using Application.Core;
using Application.Dtos;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.SpareParts.Queries.GetSparePartDetails
{
    public class GetSparePartDetailsQuery
    {
        public class Query : IRequest<Result<SparePartVm>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<SparePartVm>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<SparePartVm>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var spareParts = await _context
                    .SpareParts.AsNoTracking()
                    .Include(e => e.Equipment)
                    .Include(sf => sf.SparePartFeedbacks)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);

                var sparePartReturn = _mapper.Map<SparePartVm>(spareParts);

                return Result<SparePartVm>.Success(sparePartReturn);
            }
        }
    }
}
