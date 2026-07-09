using MediatR;
using Post.Application.Features.Tags.Responses;

namespace Post.Application.Features.Tags.Commands.UpdateTag
{
    public class UpdateTagCommand : IRequest<UpdateTagResponse>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
