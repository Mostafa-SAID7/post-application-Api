namespace Post.Application.Common.Models
{
    /// <summary>
    /// Centralized sorting parameters
    /// </summary>
    public class SortParams
    {
        public string? SortBy { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

        public bool IsDescending => SortDirection == SortDirection.Descending;
    }

    public enum SortDirection
    {
        Ascending = 0,
        Descending = 1
    }
}
