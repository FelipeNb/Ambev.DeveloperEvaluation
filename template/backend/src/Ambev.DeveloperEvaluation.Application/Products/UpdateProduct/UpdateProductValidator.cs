using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Validator for UpdateProductCommand
/// </summary>
public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    /// <summary>
    /// Initializes validation rules for UpdateProductCommand
    /// </summary>
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product ID is required");
        
        
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
