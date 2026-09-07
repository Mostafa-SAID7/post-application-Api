using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Post.Application.Features.Tags.Commands.CreateTag;
using Post.Application.Features.Tags.Commands.UpdateTag;
using Post.Application.Features.Tags.Commands.DeleteTag;
using Post.Application.Features.Tags.Queries.GetTag;
using Post.Application.Features.Tags.Queries.GetTags;
using Post.Application.Features.Tags.Responses;

namespace Post.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Get all tags. Use popularOnly=true to get the most-used tags (sorted by Count).
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetTagsResponse>> GetAll(
            [FromQuery] bool popularOnly = false,
            [FromQuery] int take = 10)
        {
            var query = new GetTagsQuery { PopularOnly = popularOnly, Take = take };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get a tag by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetTagResponse>> GetById(Guid id)
        {
            var query = new GetTagQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Create a new tag
        /// </summary>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateTagResponse>> Create(CreateTagCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing tag
        /// </summary>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateTagResponse>> Update(Guid id, [FromBody] UpdateTagCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Delete a tag by ID (soft delete)
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteTagCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
