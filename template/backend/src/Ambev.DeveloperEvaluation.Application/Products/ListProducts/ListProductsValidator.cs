using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

/// <summary>
/// Validator for ListProductsCommand
/// </summary>
public class ListProductsValidator : AbstractValidator<ListProductsCommand>
{
    /// <summary>
    /// Initializes validation rules for ListProductsCommand
    /// </summary>
    public ListProductsValidator()
    {
    }
}
