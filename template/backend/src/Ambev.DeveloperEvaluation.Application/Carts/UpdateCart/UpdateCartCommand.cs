using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

/// <summary>
/// Command for updating a cart by their ID
/// </summary>
public record UpdateCartCommand : IRequest<UpdateCartResult>
{
    public UpdateCartCommand(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// The unique identifier of the cart to retrieve
    /// </summary>
    public Guid Id { get; }
    public Guid UserId { get; set; }
    public string Branch { get; set; } = string.Empty;
    public List<CartItemCommand> Items { get; set; } = new();


    public ValidationResultDetail Validate()
    {
        var validator = new UpdateCartValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
