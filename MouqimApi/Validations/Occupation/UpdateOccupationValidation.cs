using FluentValidation;
using Models.DTOs.Occupation;

namespace MouqimApi.Validations.Occupation;

public class UpdateOccupationValidation : AbstractValidator<UpdateOccupationDto>
{
    public UpdateOccupationValidation()
    {
        RuleFor(o => o.Name).NotEmpty().WithMessage("Occupation name is required").MaximumLength(255)
            .WithMessage("Occupation name must be less than 255 characters");
        RuleFor(o => o.Id).GreaterThan(0).WithMessage("Occupation id is required");
    }
}