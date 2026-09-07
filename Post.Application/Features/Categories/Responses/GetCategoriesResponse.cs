namespace Post.Application.Features.Categories.Responses
{
    public class GetCategoriesResponse
    {
        public List<CategoryItemResponse> Items { get; set; } = [];
    }

    public class CategoryItemResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
