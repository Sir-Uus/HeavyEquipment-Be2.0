using Application.Dtos;
using FluentValidation;

namespace Application.SetValidator
{
    public class EquipmentValidator : AbstractValidator<EquipmentDto>
    {
        public EquipmentValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Type).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Brand).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Model).NotEmpty().MaximumLength(50);
            RuleFor(x => x.YearOfManufacture).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Specification).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Status).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Location).NotEmpty().MaximumLength(50);
        }
    }
}