namespace Post.Application.Common.Models
{
    /// <summary>
    /// Base class for entity-specific filter parameters
    /// </summary>
    public abstract class FilterParams
    {
        public string? SearchTerm { get; set; }

        public bool HasSearch => !string.IsNullOrWhiteSpace(SearchTerm);
    }

    /// <summary>
    /// Post-specific filter parameters
    /// </summary>
    public class PostFilterParams : FilterParams
    {
        public Guid? CategoryId { get; set; }
        public List<Guid>? TagIds { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public int? MinViewCount { get; set; }

        public bool HasCategoryFilter => CategoryId.HasValue;
        public bool HasTagFilter => TagIds?.Any() == true;
        public bool HasDateFilter => CreatedAfter.HasValue || CreatedBefore.HasValue;
        public bool HasViewCountFilter => MinViewCount.HasValue;
    }
}
