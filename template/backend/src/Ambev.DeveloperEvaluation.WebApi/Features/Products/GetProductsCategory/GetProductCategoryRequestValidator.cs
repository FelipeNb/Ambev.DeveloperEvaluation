using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProductsCategory;

/// <summary>
/// Validator for GetProductRequest
/// </summary>
public class GetProductCategoryRequestValidator : AbstractValidator<GetProductCategoryRequest>
{
    /// <summary>
    /// Initializes validation rules for GetProductRequest
    /// </summary>
    public GetProductCategoryRequestValidator()
    {
        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Product category is required");
    }
}
