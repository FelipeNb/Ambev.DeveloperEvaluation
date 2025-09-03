using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProductCategory;

/// <summary>
/// Validator for ListProductCategoryCommand
/// </summary>
public class ListProductCategoryValidator : AbstractValidator<ListProductCategoryCommand>
{
    /// <summary>
    /// Initializes validation rules for ListProductCategoryCommand
    /// </summary>
    public ListProductCategoryValidator()
    {
        
    }
}
