using FluentValidation;

namespace Post.Application.Features.Tags.Commands.CreateTag
{
    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(300).WithMessage("Description must not exceed 300 characters.")
                .When(x => x.Description != null);
        }
    }
}
