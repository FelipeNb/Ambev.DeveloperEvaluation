using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProductCategory;

/// <summary>
/// Command for retrieving a user by their ID
/// </summary>
public record ListProductCategoryCommand : IRequest<ListProductCategoryResult>
{
}
