using FluentValidation;
using Models.DTOs.EducationLevel;

namespace MouqimApi.Validations.EducationLevel;

public class AddEducationLeveValidation : AbstractValidator<AddEducationLevelDto>
{
    public AddEducationLeveValidation()
    {
        RuleFor(l => l.Name).NotEmpty().WithMessage("Education level name is required").MaximumLength(255)
            .WithMessage("Education level name must be less than 255 characters");
    }
}