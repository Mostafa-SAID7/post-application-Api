using FluentValidation;

namespace Post.Application.Features.Posts.Commands.DeletePost
{
    public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
    {
        public DeletePostCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Post ID is required.");
        }
    }
}
