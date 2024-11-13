using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.SpareParts.Queries.GetSparePartImage;

public class GetSparePartImageQuery
{
    public class Query : IRequest<Result<List<SparePartImageDetailsVm>>>
    {
        public int SparePartId { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<SparePartImageDetailsVm>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<List<SparePartImageDetailsVm>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var images = await _context
                .sparePartImages.AsNoTracking()
                .AsSplitQuery()
                .Where(x => x.SparePartId == request.SparePartId && !x.IsDeleted)
                .ToListAsync(cancellationToken);

            var sparePartImagesList = _mapper.Map<List<SparePartImageDetailsVm>>(images);

            return Result<List<SparePartImageDetailsVm>>.Success(sparePartImagesList);
        }
    }
}
