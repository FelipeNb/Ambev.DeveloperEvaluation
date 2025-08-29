using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.UserId).IsRequired();
        builder.Property(c => c.Date).IsRequired().HasColumnType("date");

        // Foreign key relationship
        builder.HasOne(c => c.User)
            .WithMany(u => u.Carts)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(c => c.UserId).HasDatabaseName("idx_carts_user_id");
        builder.HasIndex(c => c.Date).HasDatabaseName("idx_carts_date");
        builder.HasIndex(c => new { c.UserId, c.Date }).HasDatabaseName("idx_carts_user_date");
        builder.HasIndex(c => c.CreatedAt).HasDatabaseName("idx_carts_created_at");
        builder.HasIndex(c => c.UpdatedAt).HasDatabaseName("idx_carts_updated_at");
    }
}
