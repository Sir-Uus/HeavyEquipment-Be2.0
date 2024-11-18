using Application.Dtos;
using FluentValidation;

namespace Application.SetValidator;

public class TransactionDetailValidator : AbstractValidator<TransactionDetailsDto>
{
    public TransactionDetailValidator()
    {
        RuleFor(x => x.TransactionId).NotEmpty();
        RuleFor(x => x.Price).NotEmpty();
    }
}
