using FluentValidation;
using Models.DTOs.Person;

namespace MouqimApi.Validations.Person;

public class AddPersonValidation : AbstractValidator<AddPersonDto>
{
    public AddPersonValidation()
    {
        RuleFor(p => p.FullName).NotEmpty().WithMessage("Full name is required").MaximumLength(255)
            .WithMessage("Full name must be less than 255 characters");
        RuleFor(p => p.BirthDate).NotEmpty().WithMessage("Birth date is required");
        RuleFor(p => p.Gender).IsInEnum().WithMessage("Gender is required");
        RuleFor(p => p.EducationLevelId).GreaterThan(0).WithMessage("Education level id is required");
        RuleFor(p => p.OccupationId).GreaterThan(0).WithMessage("Occupation id is required");
        RuleFor(p => p.FamilyId).GreaterThan(0).WithMessage("Family id is required");
        RuleFor(p => p.RelationType).IsInEnum().WithMessage("Relation type is required");
        RuleFor(p => p.Status).IsInEnum().WithMessage("Social status is required");
        RuleFor(p => p.PhoneNo).NotEmpty().WithMessage("Phone number is required").MaximumLength(20)
            .WithMessage("Phone number must be less than 20 characters");
        RuleFor(p => p.NationalNo).NotEmpty().WithMessage("National number is required").MaximumLength(100)
            .WithMessage("National number must be less than 100 characters");
    }
}