using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Domain.Entities;

namespace Post.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Entity Framework configuration for Category entity
    /// Supports hierarchical categories with parent-child relationships
    /// </summary>
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Table configuration
            builder.ToTable("Categories", "dbo");
            builder.HasKey(e => e.Id);

            // Properties configuration
            builder.Property(e => e.Id)
                .HasColumnName("Id")
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever();

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Name")
                .HasColumnType("nvarchar(100)");

            builder.Property(e => e.Slug)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Slug")
                .HasColumnType("nvarchar(100)");

            builder.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("Description")
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

            builder.Property(e => e.ParentCategoryId)
                .HasColumnName("ParentCategoryId")
                .HasColumnType("uniqueidentifier");

            // Hierarchical relationship - self-referencing
            builder.HasOne(e => e.ParentCategory)
                .WithMany()
                .HasForeignKey(e => e.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Categories_Categories_ParentCategoryId");

            // Indexes - optimized for category lookups
            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_Categories_IsDeleted");

            builder.HasIndex(e => e.Slug)
                .IsUnique()
                .HasDatabaseName("IX_Categories_Slug");

            builder.HasIndex(e => e.ParentCategoryId)
                .HasDatabaseName("IX_Categories_ParentCategoryId");

            builder.HasIndex(e => new { e.IsDeleted, e.ParentCategoryId })
                .HasDatabaseName("IX_Categories_IsDeleted_ParentCategoryId");

            // Soft delete query filter
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
