using Application.Core;
using Application.Dtos;
using Application.SetValidator;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Persistence.Data;

namespace Application.Payments.Command.CreatePayment
{
    public class CreatePaymentCommand
    {
        public class Command : IRequest<Result<PaymentDto>>
        {
            public PaymentDto PaymentDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.PaymentDto).SetValidator(new PaymentValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<PaymentDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<PaymentDto>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var payments = _mapper.Map<Payment>(request.PaymentDto);

                _context.Payments.Add(payments);

                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!result)
                    return Result<PaymentDto>.Failure("Failed to create payment");

                var paymentDto = _mapper.Map<PaymentDto>(payments);

                return Result<PaymentDto>.Success(paymentDto);
            }
        }
    }
}
