using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// Validator for UpdateUserCommand
/// </summary>
public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    /// <summary>
    /// Initializes validation rules for UpdateUserCommand
    /// </summary>
    public UpdateUserValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User ID is required");
        
        RuleFor(user => user.Email).SetValidator(new EmailValidator());
        RuleFor(user => user.Username).NotEmpty().Length(3, 50);
        RuleFor(user => user.Password).SetValidator(new PasswordValidator());
        RuleFor(user => user.Phone).Matches(@"^\+?[1-9]\d{1,14}$");
        RuleFor(user => user.Status).NotEqual(UserStatus.Unknown);
        RuleFor(user => user.Role).NotEqual(UserRole.None);
        
        // Name validation
        RuleFor(user => user.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .Length(1, 50)
            .WithMessage("First name must be between 1 and 50 characters");
            
        RuleFor(user => user.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .Length(1, 50)
            .WithMessage("Last name must be between 1 and 50 characters");
            
        // Address validation
        RuleFor(user => user.City)
            .NotEmpty()
            .WithMessage("City is required")
            .Length(1, 100)
            .WithMessage("City must be between 1 and 100 characters");
            
        RuleFor(user => user.Street)
            .NotEmpty()
            .WithMessage("Street is required")
            .Length(1, 100)
            .WithMessage("Street must be between 1 and 100 characters");
            
        RuleFor(user => user.Number)
            .GreaterThan(0)
            .WithMessage("Number must be greater than 0");
            
        RuleFor(user => user.Zipcode)
            .NotEmpty()
            .WithMessage("Zipcode is required")
            .Length(1, 20)
            .WithMessage("Zipcode must be between 1 and 20 characters");
            
        // Optional coordinates validation
        RuleFor(user => user.Latitude)
            .Length(1, 20)
            .When(user => !string.IsNullOrEmpty(user.Latitude))
            .WithMessage("Latitude must be between 1 and 20 characters when provided");
            
        RuleFor(user => user.Longitude)
            .Length(1, 20)
            .When(user => !string.IsNullOrEmpty(user.Longitude))
            .WithMessage("Longitude must be between 1 and 20 characters when provided");
    }
}
