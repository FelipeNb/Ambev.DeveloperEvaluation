using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.CancelCart;

/// <summary>
/// Response model for CancelCart operation
/// </summary>
public class CancelCartResult
{
    /// <summary>
    /// The unique identifier of the cart
    /// </summary>
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime? Date { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime CanceldAt { get; set; }
    public long SaleNumber { get; set; }
    public string Branch { get; set; } = string.Empty;
    public bool Cancelled { get; private set; }
    public List<CartProduct> Items { get; set; } = new();

    public decimal TotalAmount { get; set; }
   
}
