using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

/// <summary>
/// Validator for ListCartsCommand
/// </summary>
public class ListCartsValidator : AbstractValidator<ListCartsCommand>
{
    /// <summary>
    /// Initializes validation rules for ListCartsCommand
    /// </summary>
    public ListCartsValidator()
    {
    }
}
