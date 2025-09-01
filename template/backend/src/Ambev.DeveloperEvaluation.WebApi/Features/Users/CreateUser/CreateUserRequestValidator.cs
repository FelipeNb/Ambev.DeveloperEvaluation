using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

/// <summary>
/// Validator for CreateUserRequest that defines validation rules for user creation.
/// </summary>
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    /// <summary>
    /// Initializes a new instance of the CreateUserRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Email: Must be valid format (using EmailValidator)
    /// - Username: Required, length between 3 and 50 characters
    /// - Password: Must meet security requirements (using PasswordValidator)
    /// - Phone: Must match international format (+X XXXXXXXXXX)
    /// - Status: Cannot be Unknown
    /// - Role: Cannot be None
    /// - FirstName: Required, length between 1 and 50 characters
    /// - LastName: Required, length between 1 and 50 characters
    /// - City: Required, length between 1 and 100 characters
    /// - Street: Required, length between 1 and 100 characters
    /// - Number: Must be greater than 0
    /// - Zipcode: Required, length between 1 and 20 characters
    /// - Latitude: Optional, length between 1 and 20 characters if provided
    /// - Longitude: Optional, length between 1 and 20 characters if provided
    /// </remarks>
    public CreateUserRequestValidator()
    {
        RuleFor(user => user.Email).SetValidator(new EmailValidator());
        RuleFor(user => user.Username).NotEmpty().Length(3, 50);
        RuleFor(user => user.Password).SetValidator(new PasswordValidator());
        RuleFor(user => user.Phone).Matches(@"^\+?[1-9]\d{1,14}$");
        RuleFor(user => user.Status).NotEqual(UserStatus.Unknown);
        RuleFor(user => user.Role).NotEqual(UserRole.None);
        
        // Name validation
        RuleFor(user => user.Name.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .Length(1, 50)
            .WithMessage("First name must be between 1 and 50 characters");
            
        RuleFor(user => user.Name.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .Length(1, 50)
            .WithMessage("Last name must be between 1 and 50 characters");
            
        // Address validation
        RuleFor(user => user.Address.City)
            .NotEmpty()
            .WithMessage("City is required")
            .Length(1, 100)
            .WithMessage("City must be between 1 and 100 characters");
            
        RuleFor(user => user.Address.Street)
            .NotEmpty()
            .WithMessage("Street is required")
            .Length(1, 100)
            .WithMessage("Street must be between 1 and 100 characters");
            
        RuleFor(user => user.Address.Number)
            .GreaterThan(0)
            .WithMessage("Number must be greater than 0");
            
        RuleFor(user => user.Address.Zipcode)
            .NotEmpty()
            .WithMessage("Zipcode is required")
            .Length(1, 20)
            .WithMessage("Zipcode must be between 1 and 20 characters");
            
        // Optional coordinates validation
        RuleFor(user => user.Address.GeoLocation.Lat)
            .Length(1, 20)
            .When(user => !string.IsNullOrEmpty(user.Address.GeoLocation.Lat))
            .WithMessage("Latitude must be between 1 and 20 characters when provided");
            
        RuleFor(user => user.Address.GeoLocation.Long)
            .Length(1, 20)
            .When(user => !string.IsNullOrEmpty(user.Address.GeoLocation.Long))
            .WithMessage("Longitude must be between 1 and 20 characters when provided");
    }
}