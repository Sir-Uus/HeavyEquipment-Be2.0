using System;
using Application.Dtos;
using FluentValidation;

namespace Application.SetValidator;

public class TransaksiValidator : AbstractValidator<TransactionDto>
{
    public TransaksiValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.TransactionDate).NotEmpty();
        RuleFor(x => x.TotalAmount)
            .NotEmpty()
            .Must(value => decimal.TryParse(value.ToString(), out _))
            .WithMessage("Amount must be numeric value");
        RuleFor(x => x.Status).NotEmpty();
    }
}
