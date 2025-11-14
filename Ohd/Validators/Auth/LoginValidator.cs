using FluentValidation;
using Ohd.DTOs.Auth;

namespace Ohd.Validators.Auth
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be left blank")
                .EmailAddress().WithMessage("invalid email");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be left blank")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long and include at leasr one special charater");
        }
    }
}