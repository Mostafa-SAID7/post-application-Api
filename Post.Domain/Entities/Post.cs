using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Domain.Entities
{
    public class Post : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public Guid? CategoryId { get; set; }
        public int ViewCount { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}

