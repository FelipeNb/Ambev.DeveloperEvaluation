using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductCategory;

/// <summary>
/// Validator for GetProductCategoryCommand
/// </summary>
public class GetProductCategoryValidator : AbstractValidator<GetProductCategoryCommand>
{
    /// <summary>
    /// Initializes validation rules for GetProductCategoryCommand
    /// </summary>
    public GetProductCategoryValidator()
    {
        
    }
}
