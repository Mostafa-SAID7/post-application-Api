using MediatR;

namespace Post.Application.Features.Tags.Commands.DeleteTag
{
    public class DeleteTagCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
