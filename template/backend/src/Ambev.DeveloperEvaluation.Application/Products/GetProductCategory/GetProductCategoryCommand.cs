using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductCategory;

/// <summary>
/// Command for retrieving a user by their ID
/// </summary>
public record GetProductCategoryCommand : IRequest<GetProductsCategoryResult>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public string? Order { get; set; }
    public string Category { get; set; }
}
