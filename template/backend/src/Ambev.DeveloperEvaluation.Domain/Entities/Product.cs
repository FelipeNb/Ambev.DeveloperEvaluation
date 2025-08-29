using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Product : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? Image { get; set; }
    public decimal RatingRate { get; set; }
    public int RatingCount { get; set; }

    // Navigation properties
    public virtual ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
