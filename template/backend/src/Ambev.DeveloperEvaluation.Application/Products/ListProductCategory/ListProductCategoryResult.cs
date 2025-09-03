namespace Ambev.DeveloperEvaluation.Application.Products.ListProductCategory;

/// <summary>
/// Response model for ListProductCategory operation
/// </summary>
public class ListProductCategoryResult
{
    public List<string> Categories { get; set; } = new ();
}
