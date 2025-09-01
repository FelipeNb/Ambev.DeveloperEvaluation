using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(p => p.Title).IsRequired().HasMaxLength(500);
        builder.Property(p => p.Price).IsRequired().HasColumnType("numeric(10,2)");
        builder.Property(p => p.Description).HasColumnType("text");
        builder.Property(p => p.Category).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Image).HasMaxLength(500);
        builder.Property(p => p.RatingRate).HasColumnType("numeric(3,2)");
        builder.Property(p => p.RatingCount).HasDefaultValue(0);

        // Constraints
        builder.HasCheckConstraint("CK_Products_Price", "Price >= 0");
        builder.HasCheckConstraint("CK_Products_RatingRate", "RatingRate >= 0 AND RatingRate <= 5");
        builder.HasCheckConstraint("CK_Products_RatingCount", "RatingCount >= 0");

        // Indexes
        builder.HasIndex(p => p.Category).HasDatabaseName("idx_products_category");
        builder.HasIndex(p => p.Price).HasDatabaseName("idx_products_price");
        builder.HasIndex(p => p.RatingRate).HasDatabaseName("idx_products_rating_rate");
        builder.HasIndex(p => p.RatingCount).HasDatabaseName("idx_products_rating_count");
        builder.HasIndex(p => new { p.Category, p.Price }).HasDatabaseName("idx_products_category_price");
        builder.HasIndex(p => p.Price).HasDatabaseName("idx_products_price_asc");
        builder.HasIndex(p => p.Price).HasDatabaseName("idx_products_price_desc");
        builder.HasIndex(p => p.RatingRate).HasDatabaseName("idx_products_rating_rate_desc");
        builder.HasIndex(p => p.CreatedAt).HasDatabaseName("idx_products_created_at");
        builder.HasIndex(p => p.UpdatedAt).HasDatabaseName("idx_products_updated_at");

        // Trigram indexes for text search
        builder.HasIndex(p => p.Title).HasDatabaseName("idx_products_title_trgm").HasMethod("gin");
        builder.HasIndex(p => p.Description).HasDatabaseName("idx_products_description_trgm").HasMethod("gin");
    }
}
