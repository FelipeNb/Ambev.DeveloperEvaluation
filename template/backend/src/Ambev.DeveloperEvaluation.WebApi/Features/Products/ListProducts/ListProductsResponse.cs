using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

/// <summary>
/// API response model for ListProducts operation
/// </summary>
public class ListProductsResponse
{
   public int TotalItems { get; set; }
   public int CurrentPage { get; set; }
   public int TotalPages { get; set; }
   public List<GetProductResponse> Items { get; set; } = new();

}
