using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

/// <summary>
/// Validator for UpdateCartCommand
/// </summary>
public class UpdateCartValidator : AbstractValidator<UpdateCartCommand>
{
    /// <summary>
    /// Initializes validation rules for UpdateCartCommand
    /// </summary>
    public UpdateCartValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Cart ID is required");
        
        RuleFor(cart => cart.UserId)
            .NotEmpty()
            .WithMessage("User ID is required");
        
        RuleFor(cart => cart.Branch)
            .NotEmpty()
            .WithMessage("Branch is required");
            
        RuleFor(cart => cart.Items).NotEmpty().NotNull()
            .WithMessage("At least one item must be in the cart");
        
        RuleFor(cart => cart.Items)
            .NotEmpty().WithMessage("At least one item must be in the cart")
            .Must(items => items.Select(i => i.ProductId).Distinct().Count() == items.Count)
            .WithMessage("Duplicate ProductIds are not allowed");
        
        RuleForEach(cart => cart.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId).NotEmpty().WithMessage("ProductId is required");
            item.RuleFor(i => i.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero");
            item.RuleFor(i => i.Quantity).LessThanOrEqualTo(20).WithMessage("Quantity must be less than 20");
        });
    }
}
