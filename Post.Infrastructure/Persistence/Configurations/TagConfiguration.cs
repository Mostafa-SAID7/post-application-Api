using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Domain.Entities;

namespace Post.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Entity Framework configuration for Tag entity
    /// Supports tagging with popularity tracking
    /// </summary>
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            // Table configuration
            builder.ToTable("Tags", "dbo");
            builder.HasKey(e => e.Id);

            // Properties configuration
            builder.Property(e => e.Id)
                .HasColumnName("Id")
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever();

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Name")
                .HasColumnType("nvarchar(50)");

            builder.Property(e => e.Slug)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Slug")
                .HasColumnType("nvarchar(50)");

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

            builder.Property(e => e.Count)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnName("Count")
                .HasColumnType("int")
                .IsRowVersion();

            // Indexes - optimized for tag queries
            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_Tags_IsDeleted");

            builder.HasIndex(e => e.Slug)
                .IsUnique()
                .HasDatabaseName("IX_Tags_Slug");

            builder.HasIndex(e => e.Count)
                .HasDatabaseName("IX_Tags_Count")
                .IsDescending();

            builder.HasIndex(e => new { e.IsDeleted, e.Count })
                .HasDatabaseName("IX_Tags_IsDeleted_Count")
                .IsDescending(false, true);

            // Soft delete query filter
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
