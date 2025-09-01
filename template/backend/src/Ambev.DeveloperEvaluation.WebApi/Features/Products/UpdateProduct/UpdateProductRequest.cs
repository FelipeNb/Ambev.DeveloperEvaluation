using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;


/// <summary>
/// API response model for UpdateProduct operation
/// </summary>
public class UpdateProductRequest
{
    /// <summary>
    /// The unique identifier of the product
    /// </summary>
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? Image { get; set; }
    public RatingProduct Rating { get; set; } = new();
    
}