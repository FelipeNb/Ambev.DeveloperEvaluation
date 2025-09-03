using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.GetCart;

/// <summary>
/// Validator for GetCartRequest that defines validation rules for cart creation.
/// </summary>
public class GetCartRequestValidator : AbstractValidator<GetCartRequest>
{
    public GetCartRequestValidator()
    {
        RuleFor(cart => cart.Id)
            .NotEmpty()
            .WithMessage("Cart ID is required");
        
    }
}