using FluentValidation;
using Ohd.DTOs.Requests;

namespace Ohd.Validators.Requests
{
    public class RequestCommentCreateValidator : AbstractValidator<RequestCommentCreateDto>
    {
        public RequestCommentCreateValidator()
        {
            RuleFor(x => x.RequestId)
                .GreaterThan(0).WithMessage("RequestId is required");

            RuleFor(x => x.AuthorUserId)
                .GreaterThan(0).WithMessage("AuthorUserId is required");

            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Comment body is required")
                .MinimumLength(3).WithMessage("Comment must have at least 3 characters")
                .MaximumLength(2000).WithMessage("Comment cannot exceed 2000 characters");
        }
    }
}