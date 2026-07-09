using FluentValidation;

namespace Post.Application.Features.Tags.Commands.UpdateTag
{
    public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
    {
        public UpdateTagCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Tag ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(300).WithMessage("Description must not exceed 300 characters.")
                .When(x => x.Description != null);
        }
    }
}
