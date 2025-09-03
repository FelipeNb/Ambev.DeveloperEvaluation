using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.UpdateCart;

/// <summary>
/// API response model for UpdateCart operation
/// </summary>
public class UpdateCartResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime? Date { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long SaleNumber { get; set; }
    public string Branch { get; set; } = string.Empty;
    public bool Cancelled { get; private set; }
    public List<CartProduct> Items { get; set; } = new();

    public decimal TotalAmount { get; private set; }
}