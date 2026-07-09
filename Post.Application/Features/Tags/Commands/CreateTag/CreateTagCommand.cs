using MediatR;
using Post.Application.Features.Tags.Responses;

namespace Post.Application.Features.Tags.Commands.CreateTag
{
    public class CreateTagCommand : IRequest<CreateTagResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
