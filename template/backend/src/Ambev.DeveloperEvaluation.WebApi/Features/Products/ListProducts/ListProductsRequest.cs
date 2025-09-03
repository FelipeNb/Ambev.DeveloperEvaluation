namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

/// <summary>
/// Request model for getting a user by ID
/// </summary>
public class ListProductsRequest
{
    public int Page { get; set; }
    public int Size { get; set; }
}
