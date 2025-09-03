using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

/// <summary>
/// Response model for GetCart operation
/// </summary>
public class GetCartResult
{
    
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime? Date { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long SaleNumber { get; set; }
    public string Branch { get; set; } = string.Empty;
    public bool Cancelled { get; private set; }
    public List<CartProduct> Items { get; set; } = new();

    public decimal TotalAmount { get; set; }
}
