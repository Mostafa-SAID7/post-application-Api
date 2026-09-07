using MediatR;
using Microsoft.AspNetCore.Mvc;
using Post.Application.Features.Categories.Commands.CreateCategory;
using Post.Application.Features.Categories.Commands.UpdateCategory;
using Post.Application.Features.Categories.Commands.DeleteCategory;
using Post.Application.Features.Categories.Queries.GetCategory;
using Post.Application.Features.Categories.Queries.GetCategories;
using Post.Application.Features.Categories.Responses;

namespace Post.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Get all categories. Use rootOnly=true to get only top-level categories,
        /// or parentCategoryId to get subcategories of a specific parent.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetCategoriesResponse>> GetAll(
            [FromQuery] Guid? parentCategoryId,
            [FromQuery] bool rootOnly = false)
        {
            var query = new GetCategoriesQuery { ParentCategoryId = parentCategoryId, RootOnly = rootOnly };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get a category by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetCategoryResponse>> GetById(Guid id)
        {
            var query = new GetCategoryQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateCategoryResponse>> Create(CreateCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateCategoryResponse>> Update(Guid id, [FromBody] UpdateCategoryCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Delete a category by ID (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteCategoryCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
