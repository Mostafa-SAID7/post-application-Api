using Post.Application.Common.Models;
using Post.Application.Common.Specifications;
using Post.Domain.Entities;

namespace Post.Application.Common.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllIncludingDeletedAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id);
        Task HardDeleteAsync(Guid id);

        // Advanced query methods
        Task<List<TEntity>> GetBySpecificationAsync(Specification<TEntity> spec);
        Task<PagedResult<TEntity>> GetPagedBySpecificationAsync(Specification<TEntity> spec);
        Task<int> CountAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate);
    }
}
