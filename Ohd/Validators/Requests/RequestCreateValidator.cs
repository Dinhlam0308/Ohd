using FluentValidation;
using Ohd.DTOs.Requests;

namespace Ohd.Validators.Requests
{
    public class RequestCreateValidator : AbstractValidator<RequestCreateDto>
    {
        public RequestCreateValidator()
        {
            RuleFor(x => x.RequestorId)
                .GreaterThan(0).WithMessage("RequestorId is required");

            RuleFor(x => x.FacilityId)
                .GreaterThan(0).WithMessage("FacilityId is required");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MinimumLength(5).WithMessage("Title must be at least 5 characters")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters")
                .When(x => x.Description != null);

            RuleFor(x => x.SeverityId)
                .GreaterThan(0).WithMessage("SeverityId is required");
        }
    }
}