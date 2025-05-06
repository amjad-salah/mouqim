using FluentValidation;
using Models.DTOs.Family;

namespace MouqimApi.Validations.Family;

public class UpdateFamilyValidation : AbstractValidator<UpdateFamilyDto>
{
    public UpdateFamilyValidation()
    {
        RuleFor(f => f.FamilyName).NotEmpty().WithMessage("Family name is required").MaximumLength(255)
            .WithMessage("Family name must be less than 255 characters");
        RuleFor(f => f.Neighbourhood).NotEmpty().WithMessage("Neighbourhood is required").MaximumLength(255)
            .WithMessage("Neighbourhood must be less than 255 characters");
        RuleFor(f => f.HousingType).IsInEnum().WithMessage("Housing type is required");
        RuleFor(f => f.IncomeStatus).IsInEnum().WithMessage("Income status is required");
        RuleFor(f => f.State).IsInEnum().WithMessage("State is required");
        RuleFor(f => f.Id).GreaterThan(0).WithMessage("Family id is required");
        RuleFor(f => f.RegisterDate).NotEmpty()
            .WithMessage("Register date is required");
    }
}