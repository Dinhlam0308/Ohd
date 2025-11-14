using FluentValidation;
using Ohd.DTOs.Requests;

namespace Ohd.Validators.Requests
{
    public class RequestChangeStatusValidator : AbstractValidator<RequestChangeStatusDto>
    {
        public RequestChangeStatusValidator()
        {
            RuleFor(x => x.ToStatusId)
                .GreaterThan(0).WithMessage("ToStatusId is required");

            RuleFor(x => x.ChangedByUserId)
                .GreaterThan(0).WithMessage("ChangedByUserId is required");

            RuleFor(x => x.Remarks)
                .MaximumLength(1000).WithMessage("Remarks cannot exceed 1000 characters");
        }
    }
}