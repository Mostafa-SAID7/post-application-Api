using Microsoft.EntityFrameworkCore;
using Post.Application.Common.Interfaces;
using Post.Domain.Entities;
using Post.Infrastructure.Persistence;

namespace Post.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Category?> GetBySlugAsync(string slug)
        {
            return await DbSet
                .Where(c => c.Slug == slug && !c.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Category>> GetRootCategoriesAsync()
        {
            return await DbSet
                .Where(c => c.ParentCategoryId == null && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Category>> GetSubCategoriesAsync(Guid parentCategoryId)
        {
            return await DbSet
                .Where(c => c.ParentCategoryId == parentCategoryId && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await DbSet
                .AnyAsync(c => c.Name == name && !c.IsDeleted);
        }
    }
}
