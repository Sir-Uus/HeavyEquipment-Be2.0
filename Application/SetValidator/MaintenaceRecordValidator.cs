using Application.Dtos;
using FluentValidation;

namespace Application.SetValidator
{
    public class MaintenaceRecordValidator : AbstractValidator<MaintenanceRecordDto>
    {
        public MaintenaceRecordValidator()
        {
            RuleFor(x => x.EquipmentId).NotEmpty();
            RuleFor(x => x.MaintenanceDate).NotEmpty();
            RuleFor(x => x.ServicedPerformed).NotEmpty().MaximumLength(50);
            RuleFor(x => x.ServicedProvider).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Cost)
                .NotEmpty()
                .Must(value => decimal.TryParse(value.ToString(), out _))
                .WithMessage("Cost must be numeric value");
            RuleFor(x => x.NextMaintenanceDue).NotEmpty();
        }
    }
}
