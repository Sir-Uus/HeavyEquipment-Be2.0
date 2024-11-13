using Application.Dtos;
using FluentValidation;

namespace Application.SetValidator
{
    public class PaymentValidator : AbstractValidator<PaymentDto>
    {
        public PaymentValidator()
        {
            RuleFor(x => x.Amount)
                .NotEmpty()
                .Must(value => decimal.TryParse(value.ToString(), out _))
                .WithMessage("Amount must be numeric value");
            RuleFor(x => x.PaymentMethod).NotEmpty().MaximumLength(10);
            RuleFor(x => x.PaymentStatus).NotEmpty().MaximumLength(10);
        }
    }
}
