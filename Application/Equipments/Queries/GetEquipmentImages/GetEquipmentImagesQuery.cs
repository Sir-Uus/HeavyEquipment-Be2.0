using System;
using Application.Core;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Equipments.Queries.GetEquipmentImages;

public class GetEquipmentImagesQuery
{
    public class Query : IRequest<Result<List<ImageDetailsVm>>>
    {
        public int EquipmentId { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<ImageDetailsVm>>>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<ImageDetailsVm>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var images = await _context
                .Images.AsNoTracking()
                .AsSplitQuery()
                .Where(x => x.EquipmentId == request.EquipmentId && !x.IsDeleted)
                .ToListAsync(cancellationToken);

            var imageVmList = _mapper.Map<List<ImageDetailsVm>>(images);

            return Result<List<ImageDetailsVm>>.Success(imageVmList);
        }
    }
}
