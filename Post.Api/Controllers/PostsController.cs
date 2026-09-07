using MediatR;
using Microsoft.AspNetCore.Mvc;
using Post.Application.Features.Posts.Responses;
using Post.Application.Features.Posts.Queries.GetPost;
using Post.Application.Features.Posts.Queries.GetPosts;
using Post.Application.Features.Posts.Queries.SearchPosts;
using Post.Application.Features.Posts.Commands.CreatePost;
using Post.Application.Features.Posts.Commands.UpdatePost;
using Post.Application.Features.Posts.Commands.DeletePost;
using Post.Application.Common.Models;

namespace Post.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Get all posts
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetPostsResponse>> GetAll()
        {
            var query = new GetPostsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Search posts with filtering, sorting, and pagination
        /// Supports: search term, category filter, tag filter, date range, view count filter
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SearchPostsResponse>> Search(
            [FromQuery] string? searchTerm,
            [FromQuery] Guid? categoryId,
            [FromQuery] List<Guid>? tagIds,
            [FromQuery] DateTime? createdAfter,
            [FromQuery] DateTime? createdBefore,
            [FromQuery] int? minViewCount,
            [FromQuery] string? sortBy = "created",
            [FromQuery] int sortDirection = 0,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new SearchPostsQuery
            {
                SearchTerm = searchTerm,
                CategoryId = categoryId,
                TagIds = tagIds,
                CreatedAfter = createdAfter,
                CreatedBefore = createdBefore,
                MinViewCount = minViewCount,
                SortBy = sortBy,
                SortDirection = (SortDirection)sortDirection,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get post by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetPostResponse>> GetById(Guid id)
        {
            var query = new GetPostQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Create a new post
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreatePostResponse>> Create(CreatePostCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing post
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdatePostResponse>> Update(Guid id, [FromBody] UpdatePostCommand command)
        {
            command.Id = id;
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Delete a post by ID
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeletePostCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}

