using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Domain.Entities;

namespace Post.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Entity Framework configuration for Post entity
    /// Defines relationships, constraints, indexes, and query filters
    /// </summary>
    public class PostConfiguration : IEntityTypeConfiguration<Domain.Entities.Post>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Post> builder)
        {
            // Table configuration
            builder.ToTable("Posts", "dbo");
            builder.HasKey(e => e.Id);

            // Properties configuration
            builder.Property(e => e.Id)
                .HasColumnName("Id")
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever();

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("Title")
                .HasColumnType("nvarchar(200)");

            builder.Property(e => e.Content)
                .IsRequired()
                .HasColumnName("Content")
                .HasColumnType("TEXT");

            builder.Property(e => e.Slug)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("Slug")
                .HasColumnType("nvarchar(200)");

            builder.Property(e => e.Summary)
                .HasMaxLength(500)
                .HasColumnName("Summary")
                .HasColumnType("nvarchar(500)");

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt")
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .HasColumnType("datetime2");

            builder.Property(e => e.DeletedAt)
                .HasColumnName("DeletedAt")
                .HasColumnType("datetime2");

            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted")
                .HasColumnType("bit");

            builder.Property(e => e.ViewCount)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnName("ViewCount")
                .HasColumnType("int");

            builder.Property(e => e.CategoryId)
                .HasColumnName("CategoryId")
                .HasColumnType("uniqueidentifier");

            // Relationships
            builder.HasOne(e => e.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Posts_Categories_CategoryId");

            builder.HasMany(e => e.Tags)
                .WithMany(t => t.Posts)
                .UsingEntity(
                    "PostTags",
                    l => l.HasOne(typeof(Domain.Entities.Tag)).WithMany().HasForeignKey("TagId").HasConstraintName("FK_PostTags_Tags_TagId"),
                    r => r.HasOne(typeof(Domain.Entities.Post)).WithMany().HasForeignKey("PostId").HasConstraintName("FK_PostTags_Posts_PostId"),
                    j => j.HasKey("PostId", "TagId").HasName("PK_PostTags"));

            // Indexes - optimized for common queries
            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_Posts_IsDeleted")
                .IncludeProperties(nameof(Domain.Entities.Post.CreatedAt), nameof(Domain.Entities.Post.ViewCount));

            builder.HasIndex(e => e.Slug)
                .IsUnique()
                .HasDatabaseName("IX_Posts_Slug");

            builder.HasIndex(e => e.CategoryId)
                .HasDatabaseName("IX_Posts_CategoryId");

            builder.HasIndex(e => e.CreatedAt)
                .HasDatabaseName("IX_Posts_CreatedAt")
                .IsDescending();

            builder.HasIndex(e => new { e.IsDeleted, e.CreatedAt })
                .HasDatabaseName("IX_Posts_IsDeleted_CreatedAt")
                .IsDescending(false, true);

            builder.HasIndex(e => e.ViewCount)
                .HasDatabaseName("IX_Posts_ViewCount")
                .IsDescending();

            // Soft delete query filter
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
