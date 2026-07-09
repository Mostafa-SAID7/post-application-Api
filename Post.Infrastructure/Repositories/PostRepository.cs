using Microsoft.EntityFrameworkCore;
using Post.Application.Common.Interfaces;
using Post.Application.Common.Models;
using Post.Application.Common.Specifications;
using Post.Domain.Entities;
using Post.Infrastructure.Persistence;

namespace Post.Infrastructure.Repositories
{
    public class PostRepository : Repository<Domain.Entities.Post>, IPostRepository
    {
        public PostRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsAsync(string slug)
        {
            return await DbSet.AnyAsync(p => p.Slug == slug && !p.IsDeleted);
        }

        public async Task<Domain.Entities.Post?> GetWithComments(Guid id)
        {
            return await DbSet.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<List<Domain.Entities.Post>> GetPopularPosts(int take = 10)
        {
            var spec = PostSpecifications.CreatePopularSpec(take);
            return await GetBySpecificationAsync(spec);
        }

        /// <summary>
        /// Search posts with filtering, sorting, and pagination
        /// </summary>
        public async Task<PagedResult<Domain.Entities.Post>> SearchAsync(
            PostFilterParams filters,
            SortParams sort,
            PaginationParams pagination)
        {
            var spec = PostSpecifications.CreateSearchSpec(filters, sort, pagination);
            return await GetPagedBySpecificationAsync(spec);
        }

        /// <summary>
        /// Get all posts in a category
        /// </summary>
        public async Task<List<Domain.Entities.Post>> GetByCategoryAsync(Guid categoryId)
        {
            var spec = PostSpecifications.CreateByCategorySpec(categoryId);
            return await GetBySpecificationAsync(spec);
        }

        /// <summary>
        /// Get all posts with a specific tag
        /// </summary>
        public async Task<List<Domain.Entities.Post>> GetByTagAsync(Guid tagId)
        {
            return await DbSet
                .Where(p => !p.IsDeleted && p.Tags.Any(t => t.Id == tagId))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}

