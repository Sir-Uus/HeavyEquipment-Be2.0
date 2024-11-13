using Application.Core;
using Application.Dtos;
using AutoMapper;
using MediatR;
using Persistence.Data;

namespace Application.SparePartFeedbacks.Command.UpdateSparePartFeedback;

public class UpdateSparePartFeedbackCommand
{
    public class Command : IRequest<Result<Unit>>
    {
        public SparePartFeedbackDto SparePartFeedbackDto { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var sparePartFeedback = await _context.SparePartFeedbacks.FindAsync(
                request.SparePartFeedbackDto.Id
            );

            if (sparePartFeedback == null)
                return null;

            _mapper.Map(request.SparePartFeedbackDto, sparePartFeedback);

            await _context.SaveChangesAsync();

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
