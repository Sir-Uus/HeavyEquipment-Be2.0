using Application.Core;
using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.SparePartFeedbacks.Command.CreateSparePartFeedback;

public class CreateSparePartFeedbackCommand
{
    public class Command : IRequest<Result<Unit>>
    {
        public SparePartFeedbackDto SparePartFeedbackDto { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var sparepart = await _context.SpareParts.FirstOrDefaultAsync(x =>
                x.Id == request.SparePartFeedbackDto.SparePartId
            );

            var user = await _context.Users.FirstOrDefaultAsync(x =>
                x.Id == request.SparePartFeedbackDto.UserId
            );

            if (sparepart == null)
            {
                return Result<Unit>.Failure("Equipment not found");
            }

            if (user == null)
            {
                return Result<Unit>.Failure("User must be registered first");
            }

            var sparePartFeedback = _mapper.Map<SparePartFeedback>(request.SparePartFeedbackDto);

            _context.SparePartFeedbacks.Add(sparePartFeedback);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
                return Result<Unit>.Failure("ailed to Create Feedback");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
