using Application.Dtos;
using Domain.Entities;
using FluentValidation;

namespace Application.SetValidator
{
    public class FeedbackValidator : AbstractValidator<FeedbackDto>
    {
        public FeedbackValidator()
        {
            RuleFor(x => x.EquipmentId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.FeedbackDate).NotEmpty();
            RuleFor(x => x.Rating).NotEmpty().InclusiveBetween(1, 5);
            RuleFor(x => x.Comment).NotEmpty();
        }
    }
}
