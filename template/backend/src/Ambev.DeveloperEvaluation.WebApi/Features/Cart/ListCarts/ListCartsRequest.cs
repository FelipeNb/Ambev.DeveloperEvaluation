namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.ListCarts;

/// <summary>
/// Request model for getting a user by ID
/// </summary>
public class ListCartsRequest
{
    public int Page { get; set; }
    public int Size { get; set; }
}
