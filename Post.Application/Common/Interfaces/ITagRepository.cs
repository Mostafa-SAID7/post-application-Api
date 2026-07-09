using Post.Domain.Entities;

namespace Post.Application.Common.Interfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<Tag?> GetBySlugAsync(string slug);
        Task<Tag?> GetByNameAsync(string name);
        Task<List<Tag>> GetPopularTagsAsync(int take = 10);
        Task<bool> ExistsAsync(string name);
    }
}
