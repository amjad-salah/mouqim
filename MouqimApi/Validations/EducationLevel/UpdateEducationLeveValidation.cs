using FluentValidation;
using Models.DTOs.EducationLevel;

namespace MouqimApi.Validations.EducationLevel;

public class UpdateEducationLeveValidation : AbstractValidator<UpdateEducationLevelDto>
{
    public UpdateEducationLeveValidation()
    {
        RuleFor(l => l.Name).NotEmpty().WithMessage("Education level name is required").MaximumLength(255)
            .WithMessage("Education level name must be less than 255 characters");

        RuleFor(l => l.Id).GreaterThan(0).WithMessage("Education level id is required");
    }
}