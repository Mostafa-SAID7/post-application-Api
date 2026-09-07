using Microsoft.EntityFrameworkCore;
using Post.Application.Common.Interfaces;
using Post.Application.Common.Models;
using Post.Application.Common.Specifications;
using Post.Domain.Entities;
using Post.Infrastructure.Persistence;

namespace Post.Infrastructure.Repositories
{
    public class Repository<T>(AppDbContext context) : IRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext Context = context;
        protected readonly DbSet<T> DbSet = context.Set<T>();

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await DbSet
                .Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllIncludingDeletedAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            DbSet.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await DbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                entity.Delete();
                DbSet.Update(entity);
            }
        }

        public async Task HardDeleteAsync(Guid id)
        {
            var entity = await DbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                DbSet.Remove(entity);
            }
        }

        /// <summary>
        /// Get entities using a specification
        /// </summary>
        public async Task<List<T>> GetBySpecificationAsync(Specification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        /// <summary>
        /// Get paged entities using a specification
        /// </summary>
        public async Task<PagedResult<T>> GetPagedBySpecificationAsync(Specification<T> spec)
        {
            var totalCount = await ApplySpecification(spec).CountAsync();
            var items = await ApplySpecification(spec)
                .Skip(spec.Skip)
                .Take(spec.Take)
                .ToListAsync();

            var pagination = new PaginationParams
            {
                PageNumber = (spec.Skip / spec.Take) + 1,
                PageSize = spec.Take
            };

            return new PagedResult<T>(items, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        /// <summary>
        /// Count entities matching a predicate
        /// </summary>
        public async Task<int> CountAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await DbSet.CountAsync(predicate);
        }

        /// <summary>
        /// Apply specification to queryable
        /// </summary>
        protected IQueryable<T> ApplySpecification(Specification<T> spec)
        {
            var query = DbSet.AsQueryable();

            // Apply criteria
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            // Apply includes
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            // Apply string-based includes
            query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

            // Apply ordering
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            return query;
        }
    }
}

