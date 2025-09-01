namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProductCategory;


/// <summary>
/// API response model for ListProductCategory operation
/// </summary>
public class ListProductCategoryResponse
{
    public List<string> Categories { get; set; } = new();
    
}
