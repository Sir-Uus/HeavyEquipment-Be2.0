using Application.Core;
using MediatR;
using Persistence.Data;

namespace Application.SpareParts.Command.DeleteSparePart
{
    public class DeleteSparePartCommand
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var sparePart = await _context.SpareParts.FindAsync(request.Id);

                if (sparePart == null)
                    return null;

                sparePart.IsDeleted = true;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                    return Result<Unit>.Failure("Failed to delete spareparts");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
