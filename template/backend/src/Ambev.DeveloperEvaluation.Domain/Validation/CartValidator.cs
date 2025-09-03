using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class CartValidator : AbstractValidator<Cart>
{
    public CartValidator()
    {
        RuleFor(cart => cart.UserId).NotNull().NotEmpty()
            .WithMessage("Refered user must be identified");

        RuleFor(cart => cart.SaleNumber).NotNull().NotEmpty()
            .WithMessage("Sale number must be specified");
        RuleFor(cart => cart.Branch).NotNull().NotEmpty()
            .WithMessage("Branch must be specified");
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
            item.RuleFor(i => i.Quantity).LessThanOrEqualTo(20).WithMessage("Quantity must be less than or equal to 20");
            item.RuleFor(i => i.UnitPrice).GreaterThan(0).WithMessage("Unit price must be greater than zero");
        });
        
        RuleFor(cart => cart.TotalAmount).GreaterThan(0)
            .WithMessage("The total amount has to be greater than zero");
        
        
    }
}
