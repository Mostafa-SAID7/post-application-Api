using Post.Domain.Entities;

namespace Post.Application.Common.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetBySlugAsync(string slug);
        Task<List<Category>> GetRootCategoriesAsync();
        Task<List<Category>> GetSubCategoriesAsync(Guid parentCategoryId);
        Task<bool> ExistsAsync(string name);
    }
}
