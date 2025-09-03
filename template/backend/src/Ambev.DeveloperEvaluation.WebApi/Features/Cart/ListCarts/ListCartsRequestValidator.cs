using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.ListCarts;

/// <summary>
/// Validator for GetUserRequest
/// </summary>
public class ListCartsRequestValidator : AbstractValidator<ListCartsRequest>
{
    /// <summary>
    /// Initializes validation rules for GetUserRequest
    /// </summary>
    public ListCartsRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be grater than 0");
        
        RuleFor(x => x.Size)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Size must be grater than 0");
    }
}
