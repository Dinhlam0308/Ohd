using FluentValidation;
using Ohd.DTOs.Auth;

namespace Ohd.Validators.Auth
{
    public class ChangePasswordFirstLoginValidator : AbstractValidator<ChangePasswordFirstLoginRequest>
    {
        public ChangePasswordFirstLoginValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Invalid UserId");

            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Old Password cannot be left blank");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password cannot be left blank")
                .MinimumLength(8).WithMessage("New Password must be at least 8 characters long and include at leasr one special charater")
                .Matches("[A-Z]").WithMessage("The new password must contain at least 1 uppercase character")
                .Matches("[a-z]").WithMessage("The new password must contain at least 1 lowercase character")
                .Matches("[0-9]").WithMessage("The new password must contain at least 1 digit")
                .Matches("[^a-zA-Z0-9]").WithMessage("The new password must contain at least 1 special character");
            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty().WithMessage("Confirm Password cannot be left blank")
                .Equal(x => x.NewPassword).WithMessage("The new password do not match");
        }
    }
}