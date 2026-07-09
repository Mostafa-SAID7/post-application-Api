using Microsoft.EntityFrameworkCore;
using Post.Application.Common.Interfaces;
using Post.Domain.Entities;
using Post.Infrastructure.Persistence;

namespace Post.Infrastructure.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Tag?> GetBySlugAsync(string slug)
        {
            return await DbSet
                .Where(t => t.Slug == slug && !t.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<Tag?> GetByNameAsync(string name)
        {
            return await DbSet
                .Where(t => t.Name == name && !t.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Tag>> GetPopularTagsAsync(int take = 10)
        {
            return await DbSet
                .Where(t => !t.IsDeleted)
                .OrderByDescending(t => t.Count)
                .Take(take)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await DbSet
                .AnyAsync(t => t.Name == name && !t.IsDeleted);
        }
    }
}
