using FluentValidation;
using Models.DTOs.User;

namespace MouqimApi.Validations.User;

public class UpdateUserValidation : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidation()
    {
        RuleFor(u => u.Id).GreaterThan(0).WithMessage("User id is required");
        RuleFor(u => u.FullName).NotEmpty().WithMessage("Full name is required").MaximumLength(255)
            .WithMessage("Full name must be less than 255 characters");
        RuleFor(u => u.Username).NotEmpty().WithMessage("Username is required").MaximumLength(255)
            .WithMessage("Username must be less than 255 characters");
        RuleFor(u => u.Password).NotEmpty().WithMessage("Password is required").MaximumLength(255)
            .WithMessage("Password must be less than 255 characters");
        RuleFor(u => u.Role).IsInEnum().WithMessage("Role is required");
    }
}