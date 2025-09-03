using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        
        RuleFor(user => user.Title).NotEmpty().Length(3, 500);
        RuleFor(user => user.Category).NotEmpty().Length(3, 100);
        RuleFor(user => user.Price).NotEmpty().GreaterThan(0);
        
        RuleFor(user => user.Description).Length(3, 50);
        RuleFor(user => user.Image).MaximumLength(500);
        
        RuleFor(user => user.Title)
            .NotEmpty()
            .MinimumLength(3).WithMessage("Title must be at least 3 characters long.")
            .MaximumLength(500).WithMessage("Title cannot be longer than 500 characters.");
        
        RuleFor(user => user.Description)
            .MinimumLength(3).WithMessage("Description must be at least 3 characters long.")
            .MaximumLength(50).WithMessage("Description cannot be longer than 50 characters.");
        
        RuleFor(user => user.Image)
            .MaximumLength(500).WithMessage("Image path cannot be longer than 500 characters.");
        
        RuleFor(user => user.Category)
            .NotEmpty()
            .MinimumLength(3).WithMessage("Category must be at least 3 characters long.")
            .MaximumLength(100).WithMessage("Category cannot be longer than 100 characters.");
        
        RuleFor(user => user.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");
        
    }
}
