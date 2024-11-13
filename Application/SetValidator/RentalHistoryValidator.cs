using Application.Dtos;
using FluentValidation;

namespace Application.SetValidator
{
    public class RentalHistoryValidator : AbstractValidator<RentalHistoryDto>
    {
        public RentalHistoryValidator()
        {
            RuleFor(x => x.EquipmentId).NotEmpty();
            RuleFor(x => x.RenterId).NotEmpty();
            RuleFor(x => x.Invoice).NotEmpty().MaximumLength(20);
            RuleFor(x => x.RentalStartDate).NotEmpty();
            RuleFor(x => x.RentalEndDate).NotEmpty();
            RuleFor(x => x.RentalCost)
                .NotEmpty()
                .Must(value => decimal.TryParse(value.ToString(), out _))
                .WithMessage("Rental Cost must be numeric value");
            RuleFor(x => x.Location).NotEmpty().MaximumLength(30);
        }
    }
}
