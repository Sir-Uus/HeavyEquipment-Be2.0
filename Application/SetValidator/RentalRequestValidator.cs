using System.Data;
using Application.Dtos;
using FluentValidation;

namespace Application.SetValidator
{
    public class RentalRequestValidator : AbstractValidator<RentalRequestDto>
    {
        public RentalRequestValidator()
        {
            RuleFor(x => x.EquipmentId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Invoice).NotEmpty().MaximumLength(20);
            RuleFor(x => x.StarDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty();
            RuleFor(x => x.Status).NotEmpty().MaximumLength(10);
        }
    }
}
