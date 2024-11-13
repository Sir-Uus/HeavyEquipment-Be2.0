using System;
using Application.Core;
using AutoMapper;
using MediatR;
using Persistence.Data;

namespace Application.SparePartFeedbacks.Command.DeleteSparePartFeedback;

public class DeleteSparePartFeedbackCommand
{
    public class Command : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var sparePartFeedback = await _context.SparePartFeedbacks.FindAsync(request.Id);

            if (sparePartFeedback == null)
                return null;

            sparePartFeedback.IsDeleted = true;

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return Result<Unit>.Failure("Failed to Delete Sparepart Feedback");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
