using System.Text;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

/// <summary>
/// Handler for processing UpdateCartCommand requests
/// </summary>
public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, UpdateCartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Initializes a new instance of UpdateCartHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="passwordHasher"></param>
    /// <param name="productRepository"></param>
    /// <param name="userRepository"></param>
    public UpdateCartHandler(
        ICartRepository cartRepository,
        IMapper mapper,
        IPasswordHasher passwordHasher, 
        IProductRepository productRepository, 
        IUserRepository userRepository)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _productRepository = productRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Handles the UpdateCartCommand request
    /// </summary>
    /// <param name="request">The UpdateCart command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cart details if found</returns>
    public async Task<UpdateCartResult> Handle(UpdateCartCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await new UpdateCartValidator().ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var cartStored = await _cartRepository.GetByIdAsync(command.Id);
        if(cartStored is null)
            throw new KeyNotFoundException($"Cart was not found with ID {command.Id}");
        
        if (!await _userRepository.UserExistsAsync(command.UserId))
            throw new KeyNotFoundException($"User with ID {command.UserId} do not exists");

        StringBuilder stringBuilder = new StringBuilder();

        foreach (var prod in command.Items)
        {
            if (!await _productRepository.ProductExistsAsync(prod.ProductId))
                stringBuilder.Append($"Product with ID {prod.ProductId} do not exists");
        }
        
        if (stringBuilder.Length > 0)
            throw new KeyNotFoundException(stringBuilder.ToString());
        
        cartStored.UpdatedAt = DateTime.UtcNow;
        cartStored.Branch = command.Branch;
        cartStored.UserId = command.UserId;
        foreach (var item in command.Items)
        {
            var existingItem = cartStored.Items
                .FirstOrDefault(i => i.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity = item.Quantity;
            }
            else
            {
                cartStored.Items.Add(new CartProduct
                {
                    Id = Guid.NewGuid(),
                    CartId = cartStored.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                });
            }
        }
        
        var itemsToRemove = cartStored.Items
            .Where(i => !command.Items.Any(x => x.ProductId == i.ProductId))
            .ToList();

        foreach (var removeItem in itemsToRemove)
        {
            cartStored.Items.Remove(removeItem);
        }

        cartStored.Items.ForEach( async prod => prod.UnitPrice = await _productRepository.GetPriceByIdAsync(prod.ProductId));
        
        await _cartRepository.UpdateAsync(cartStored, cancellationToken);
        var result = _mapper.Map<UpdateCartResult>(cartStored);
        return result;
    }
}
