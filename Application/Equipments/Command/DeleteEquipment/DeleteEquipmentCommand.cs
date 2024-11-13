using Application.Core;
using MediatR;
using Persistence.Data;

namespace Application.Equipments.Command.DeleteEquipment
{
    public class DeleteEquipmentCommand
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
                var equipment = await _context.Equipments.FindAsync(request.Id);

                if (equipment == null)
                    return null;

                equipment.IsDeleted = true;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                    return Result<Unit>.Failure("Failed to delete the Euipment");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
