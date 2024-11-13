using Application.Core;
using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Payments.Command.UpdatePayment
{
    public class UpdatePaymentCommand
    {
        public class Command : IRequest<Result<Unit>>
        {
            public PaymentDto PaymentDto { get; set; }
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

            public async Task<Result<Unit>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var payment = await _context
                    .Payments.Include(p => p.Transaction)
                    .ThenInclude(t => t.TransactionDetails)
                    .Include(p => p.RentalRequest)
                    .ThenInclude(r => r.Equipment)
                    .FirstAsync(p => p.Id == request.PaymentDto.Id);

                if (payment == null)
                    return Result<Unit>.Failure("Payment not found");

                _mapper.Map(request.PaymentDto, payment);

                if (payment.RentalRequest.StarDate <= DateTime.UtcNow)
                {
                    payment.PaymentStatus = "On Rented";
                }

                if (payment.RentalRequest.EndDate <= DateTime.UtcNow)
                {
                    payment.PaymentStatus = "Done";

                    var rentalHistory = new RentalHistory
                    {
                        EquipmentId = payment.RentalRequest.EquipmentId,
                        RenterId = payment.RentalRequest.UserId,
                        Invoice = payment.RentalRequest.Invoice,
                        RentalStartDate = payment.RentalRequest.StarDate,
                        RentalEndDate = payment.RentalRequest.EndDate,
                        RentalCost = payment.Amount,
                        Location = payment.RentalRequest.Equipment.Location
                    };

                    await _context.RentalHistories.AddAsync(rentalHistory);
                }

                var result = await _context.SaveChangesAsync();

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
