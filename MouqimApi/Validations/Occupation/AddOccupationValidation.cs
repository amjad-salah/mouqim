using FluentValidation;
using Models.DTOs.Occupation;

namespace MouqimApi.Validations.Occupation;

public class AddOccupationValidation : AbstractValidator<AddOccupationDto>
{
    public AddOccupationValidation()
    {
        RuleFor(o => o.Name).NotEmpty().WithMessage("Occupation name is required").MaximumLength(255)
            .WithMessage("Occupation name must be less than 255 characters");
    }
}