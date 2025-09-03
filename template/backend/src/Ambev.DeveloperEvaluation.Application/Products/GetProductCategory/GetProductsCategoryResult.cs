using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductCategory;

/// <summary>
/// Response model for GetProductCategory operation
/// </summary>
public class GetProductsCategoryResult
{
    public List<Product> Items { get; set; }
   
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
