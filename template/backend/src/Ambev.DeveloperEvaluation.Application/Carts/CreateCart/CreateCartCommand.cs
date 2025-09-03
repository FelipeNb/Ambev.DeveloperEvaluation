using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CartItemCommand
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    
}

public class CreateCartCommand : IRequest<CreateCartResult>
{
    
    public Guid UserId { get; set; }
    public string Branch { get; set; } = string.Empty;
    public List<CartItemCommand> Items { get; set; } = new();

    public ValidationResultDetail Validate()
    {
        var validator = new CreateCartCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}