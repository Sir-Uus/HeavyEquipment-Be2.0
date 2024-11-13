using Application.Core;
using MediatR;
using Persistence.Data;

namespace Application.RentalRequests.Command.DeleteRentalRequest
{
    public class DeleteRentalRequestCommand
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
                var rentalRequest = await _context.RentalRequests.FindAsync(request.Id);

                if (rentalRequest == null)
                    return null;

                rentalRequest.IsDeleted = true;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                    return Result<Unit>.Failure("Failed to Delete rental request");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
