using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

/// <summary>
/// Command for retrieving a cart by their ID
/// </summary>
public record ListCartsCommand : IRequest<ListCartsResult>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public string? Order { get; set; }
}
