using Post.Application.Common.Models;
using Post.Domain.Entities;

namespace Post.Application.Common.Interfaces
{
    public interface IPostRepository : IRepository<Domain.Entities.Post>
    {
        Task<bool> ExistsAsync(string slug);
        Task<Domain.Entities.Post?> GetByIdWithTagsAsync(Guid id);
        Task<Domain.Entities.Post?> GetWithComments(Guid id);
        Task<List<Domain.Entities.Post>> GetPopularPosts(int take = 10);

        // Search and filter
        Task<PagedResult<Domain.Entities.Post>> SearchAsync(
            PostFilterParams filters,
            SortParams sort,
            PaginationParams pagination);

        Task<List<Domain.Entities.Post>> GetByCategoryAsync(Guid categoryId);
        Task<List<Domain.Entities.Post>> GetByTagAsync(Guid tagId);
    }
}
