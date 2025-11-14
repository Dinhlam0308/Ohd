using FluentValidation;
using Ohd.DTOs.Auth;

namespace Ohd.Validators.Auth
{
    public class AdminCreateUserValidator : AbstractValidator<AdminCreateUserRequest>
    {
        public AdminCreateUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be left blank")
                .EmailAddress().WithMessage("invalid email ");

            RuleFor(x => x.Username)
                .MaximumLength(100).WithMessage("Username must not exceed 100 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.Username));

            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("RoleId > 0")
                .When(x => x.RoleId.HasValue);
        }
    }
}