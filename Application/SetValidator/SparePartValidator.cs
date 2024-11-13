using Application.Dtos;
using FluentValidation;

namespace Application.SetValidator
{
    public class SparePartValidator : AbstractValidator<SparePartsDto>
    {
        public SparePartValidator()
        {
            RuleFor(x => x.EquipmentId).NotEmpty();
            RuleFor(x => x.PartName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.PartNumber).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Manufacturer).NotEmpty().MaximumLength(20);
            RuleFor(x => x.AvailabilityStatus).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Price)
                .NotEmpty()
                .Must(p => p > 0)
                .WithMessage("Price must be a positive number.")
                .Must(value => decimal.TryParse(value.ToString(), out _))
                .WithMessage("Price must be numeric value");
            ;
        }
    }
}
