using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

/// <summary>
/// Command for retrieving a user by their ID
/// </summary>
public record ListProductsCommand : IRequest<ListProductsResult>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public string? Order { get; set; }
}
