using Post.Application.Common.Models;
using Post.Domain.Entities;

namespace Post.Application.Common.Specifications
{
    /// <summary>
    /// Reusable Post specifications for search, filter, and sort
    /// </summary>
    public class PostSpecifications
    {
        /// <summary>
        /// Build a specification with search, filter, sort, and pagination
        /// </summary>
        public static Specification<Post.Domain.Entities.Post> CreateSearchSpec(
            PostFilterParams filters,
            SortParams sort,
            PaginationParams pagination)
        {
            return new PostSearchSpecification(filters, sort, pagination);
        }

        /// <summary>
        /// Get popular posts (sorted by view count)
        /// </summary>
        public static Specification<Post.Domain.Entities.Post> CreatePopularSpec(int take = 10)
        {
            return new PostPopularSpecification(take);
        }

        /// <summary>
        /// Get posts by category
        /// </summary>
        public static Specification<Post.Domain.Entities.Post> CreateByCategorySpec(Guid categoryId)
        {
            return new PostByCategorySpecification(categoryId);
        }
    }

    /// <summary>
    /// Posts search with advanced filtering
    /// </summary>
    internal class PostSearchSpecification : Specification<Post.Domain.Entities.Post>
    {
        public PostSearchSpecification(PostFilterParams filters, SortParams sort, PaginationParams pagination)
        {
            // Base criteria: exclude soft-deleted posts
            Criteria = p => !p.IsDeleted;

            // Add category filter
            if (filters.HasCategoryFilter)
            {
                var categoryId = filters.CategoryId!.Value;
                Criteria = Criteria.And(p => p.CategoryId == categoryId);
            }

            // Add search term (title and content)
            if (filters.HasSearch)
            {
                var searchTerm = filters.SearchTerm!.ToLower();
                Criteria = Criteria.And(p =>
                    p.Title.ToLower().Contains(searchTerm) ||
                    p.Content.ToLower().Contains(searchTerm) ||
                    (p.Summary != null && p.Summary.ToLower().Contains(searchTerm)));
            }

            // Add tag filter
            if (filters.HasTagFilter)
            {
                var tagIds = filters.TagIds!;
                Criteria = Criteria.And(p => p.Tags.Any(t => tagIds.Contains(t.Id)));
            }

            // Add date filters
            if (filters.HasDateFilter)
            {
                if (filters.CreatedAfter.HasValue)
                    Criteria = Criteria.And(p => p.CreatedAt >= filters.CreatedAfter.Value);

                if (filters.CreatedBefore.HasValue)
                    Criteria = Criteria.And(p => p.CreatedAt <= filters.CreatedBefore.Value);
            }

            // Add view count filter
            if (filters.HasViewCountFilter)
            {
                Criteria = Criteria.And(p => p.ViewCount >= filters.MinViewCount!.Value);
            }

            // Apply sorting
            ApplySorting(sort);

            // Apply pagination
            pagination.WithValidation();
            ApplyPaging(pagination.GetSkip(), pagination.PageSize);
        }

        private void ApplySorting(SortParams sort)
        {
            if (string.IsNullOrWhiteSpace(sort.SortBy))
            {
                // Default sort by CreatedAt descending
                ApplyOrderingDescending(p => p.CreatedAt);
                return;
            }

            var sortField = sort.SortBy.ToLower();
            if (sort.IsDescending)
            {
                ApplyOrderingDescending(sortField switch
                {
                    "title" => p => p.Title,
                    "created" or "createdat" => p => p.CreatedAt,
                    "updated" or "updatedat" => p => p.UpdatedAt ?? p.CreatedAt,
                    "views" or "viewcount" => p => p.ViewCount,
                    _ => p => p.CreatedAt
                });
            }
            else
            {
                ApplyOrdering(sortField switch
                {
                    "title" => p => p.Title,
                    "created" or "createdat" => p => p.CreatedAt,
                    "updated" or "updatedat" => p => p.UpdatedAt ?? p.CreatedAt,
                    "views" or "viewcount" => p => p.ViewCount,
                    _ => p => p.CreatedAt
                });
            }
        }
    }

    /// <summary>
    /// Popular posts specification
    /// </summary>
    internal class PostPopularSpecification : Specification<Post.Domain.Entities.Post>
    {
        public PostPopularSpecification(int take)
        {
            Criteria = p => !p.IsDeleted;
            ApplyOrderingDescending(p => p.ViewCount);
            ApplyPaging(0, take);
        }
    }

    /// <summary>
    /// Posts by category specification
    /// </summary>
    internal class PostByCategorySpecification : Specification<Post.Domain.Entities.Post>
    {
        public PostByCategorySpecification(Guid categoryId)
        {
            Criteria = p => !p.IsDeleted && p.CategoryId == categoryId;
            ApplyOrderingDescending(p => p.CreatedAt);
        }
    }
}
