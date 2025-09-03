using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

/// <summary>
/// Response model for ListProducts operation
/// </summary>
public class ListProductsResult
{
   public List<Product> Items { get; set; }
   
   public int TotalItems { get; set; }
   public int CurrentPage { get; set; }
   public int TotalPages { get; set; }
}
