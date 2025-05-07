using FluentValidation;
using Models.DTOs.User;

namespace MouqimApi.Validations.User;

public class LoginValidation : AbstractValidator<LoginDto>
{
    public LoginValidation()
    {
        RuleFor(r => r.Username).NotEmpty().WithMessage("Username is required");
        RuleFor(r => r.Password).NotEmpty().WithMessage("Password is required");
    }
}