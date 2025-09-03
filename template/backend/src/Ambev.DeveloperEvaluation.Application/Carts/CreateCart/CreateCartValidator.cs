using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

/// <summary>
/// Validator for CreateCartCommand that defines validation rules for cart creation command.
/// </summary>
public class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
{
    public CreateCartCommandValidator()
    {
        // Name validation
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