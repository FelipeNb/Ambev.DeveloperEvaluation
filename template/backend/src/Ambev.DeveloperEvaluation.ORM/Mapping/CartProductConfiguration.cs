using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class CartProductConfiguration : IEntityTypeConfiguration<CartProduct>
{
    public void Configure(EntityTypeBuilder<CartProduct> builder)
    {
        builder.ToTable("CartProducts");

        builder.HasKey(cp => cp.Id);
        builder.Property(cp => cp.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(cp => cp.CartId).IsRequired();
        builder.Property(cp => cp.ProductId).IsRequired();
        builder.Property(cp => cp.Quantity).IsRequired();
        
        builder.Property(cp => cp.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(cp => cp.DiscountPercent).HasColumnType("decimal(18,2)");
        builder.Property(cp => cp.Total).HasColumnType("decimal(18,2)");

        
        // Foreign key relationships
        builder.HasOne(cp => cp.Cart)
            .WithMany(c => c.Items)
            .HasForeignKey(cp => cp.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cp => cp.Product)
            .WithMany(p => p.CartProducts)
            .HasForeignKey(cp => cp.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(cp => cp.ProductId);
        
        // Unique constraint to prevent duplicate products in the same cart
        builder.HasIndex(cp => new { cp.CartId, cp.ProductId })
            .IsUnique()
            .HasDatabaseName("uk_cart_product");

        // Constraints
        builder.HasCheckConstraint("CK_CartProducts_Quantity", "Quantity > 0");
        builder.HasCheckConstraint("CK_CartProducts_Quantity_Max", "Quantity <= 20");

        // Indexes
        builder.HasIndex(cp => cp.CartId).HasDatabaseName("idx_cart_products_cart_id");
        builder.HasIndex(cp => cp.ProductId).HasDatabaseName("idx_cart_products_product_id");
        builder.HasIndex(cp => cp.Quantity).HasDatabaseName("idx_cart_products_quantity");
    }
}
