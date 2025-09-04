namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.Common;

public class CartItem
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}