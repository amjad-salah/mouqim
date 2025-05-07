using FluentValidation;
using Models.DTOs.User;

namespace MouqimApi.Validations.User;

public class ResetPasswordValidation : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordValidation()
    {
        RuleFor(u => u.Id).GreaterThan(0).WithMessage("User id is required");
        RuleFor(u => u.Password).NotEmpty().WithMessage("Password is required").MaximumLength(255)
            .WithMessage("Password must be less than 255 characters");
    }
}