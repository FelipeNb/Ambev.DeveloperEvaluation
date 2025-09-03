using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.CancelCart;

/// <summary>
/// Validator for CancelCartRequest that defines validation rules for cart creation.
/// </summary>
public class CancelCartRequestValidator : AbstractValidator<CancelCartRequest>
{
    public CancelCartRequestValidator()
    {
        RuleFor(cart => cart.Id)
            .NotEmpty()
            .WithMessage("Cart ID is required");
        
            
    }
}