using System;
using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.SpareParts.Queries.GetSparePartAll;

public class GetSparePartAllQuery
{
    public class Query : IRequest<Result<List<SparePartVm>>> { }

    public class Handler : IRequestHandler<Query, Result<List<SparePartVm>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<SparePartVm>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var totalItems = await _context.SpareParts.CountAsync(cancellationToken);

            var sparePart = await _context
                .SpareParts.AsNoTracking()
                .AsSplitQuery()
                .Include(e => e.Equipment)
                .ToListAsync(cancellationToken);

            var sparePartReturn = _mapper.Map<List<SparePartVm>>(sparePart);

            return Result<List<SparePartVm>>.Success(sparePartReturn);
        }
    }
}
