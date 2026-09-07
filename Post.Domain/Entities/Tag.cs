namespace Post.Domain.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Count { get; set; }
        public virtual ICollection<Post> Posts { get; set; } = [];
    }
}
