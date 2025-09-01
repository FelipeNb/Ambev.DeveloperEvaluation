using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProductsCategory;

/// <summary>
/// API response model for GetProductCategory operation
/// </summary>
public class GetProductCategoryResponse
{
   public int TotalItems { get; set; }
   public int CurrentPage { get; set; }
   public int TotalPages { get; set; }
   public List<GetProductResponse> Items { get; set; } = new();

}
