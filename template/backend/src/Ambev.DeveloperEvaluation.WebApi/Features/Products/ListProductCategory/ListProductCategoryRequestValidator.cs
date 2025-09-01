using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProductCategory;

/// <summary>
/// Validator for ListProductCategoryRequest
/// </summary>
public class ListProductCategoryRequestValidator : AbstractValidator<ListProductCategoryRequest>
{
    /// <summary>
    /// Initializes validation rules for ListProductCategoryRequest
    /// </summary>
    public ListProductCategoryRequestValidator()
    {
    }
}
