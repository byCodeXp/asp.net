using Api.Models.Entities;
using FluentValidation;

namespace Api.Validators
{
    public class FishDtoValidator : AbstractValidator<FishDto>
    {
        public FishDtoValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty();

            RuleFor(m => m.Color)
                .NotEmpty();

            RuleFor(m => m.Weight)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
