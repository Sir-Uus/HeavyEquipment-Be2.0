using Application.Core;
using MediatR;
using Persistence.Data;

namespace Application.RentalHistories.Command.DeleteRentalHistories
{
    public class DeleteRentalHistoriesCommand
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
                var rentalHistory = await _context.RentalHistories.FindAsync(request.Id);

                if (rentalHistory == null)
                    return null;

                rentalHistory.IsDeleted = true;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                    return Result<Unit>.Failure("Failed to delete rental history");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
