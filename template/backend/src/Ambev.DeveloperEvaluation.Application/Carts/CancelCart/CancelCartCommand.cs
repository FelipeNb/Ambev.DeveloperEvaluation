using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CancelCart;

/// <summary>
/// Command for updating a cart by their ID
/// </summary>
public record CancelCartCommand : IRequest<CancelCartResult>
{
    public CancelCartCommand(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// The unique identifier of the cart to retrieve
    /// </summary>
    public Guid Id { get; }


    public ValidationResultDetail Validate()
    {
        var validator = new CancelCartValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
