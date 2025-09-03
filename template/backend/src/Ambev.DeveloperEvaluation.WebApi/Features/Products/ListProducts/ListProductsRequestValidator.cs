using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

/// <summary>
/// Validator for GetUserRequest
/// </summary>
public class ListProductsRequestValidator : AbstractValidator<ListProductsRequest>
{
    /// <summary>
    /// Initializes validation rules for GetUserRequest
    /// </summary>
    public ListProductsRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be grater than 0");
        
        RuleFor(x => x.Size)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Size must be grater than 0");
    }
}
