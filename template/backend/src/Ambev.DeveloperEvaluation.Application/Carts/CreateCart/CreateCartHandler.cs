using System.Text;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

/// <summary>
/// Handler for processing CreateCartCommand requests
/// </summary>
public class CreateCartHandler : IRequestHandler<CreateCartCommand, CreateCartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CreateCartHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CreateCartHandler(ICartRepository cartRepository, IMapper mapper, IUserRepository userRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _productRepository = productRepository;
    }

    /// <summary>
    /// Handles the CreateCartCommand request
    /// </summary>
    /// <param name="command">The CreateCart command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created cart details</returns>
    public async Task<CreateCartResult> Handle(CreateCartCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateCartCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        var cart = _mapper.Map<Cart>(command);
        cart.Date = DateTime.Now;
        cart.UpdatedAt = DateTime.UtcNow;
        cart.SaleNumber = await _cartRepository.NextSaleSequenceAsync();
        if (!await _userRepository.UserExistsAsync(cart.UserId))
            throw new KeyNotFoundException($"User with ID {command.UserId} do not exists");

        StringBuilder stringBuilder = new StringBuilder();

        foreach (var prod in cart.Items)
        {
            if (!await _productRepository.ProductExistsAsync(prod.ProductId))
                stringBuilder.Append($"Product with ID {prod.ProductId} do not exists");
            else
            {
                prod.UnitPrice = await _productRepository.GetPriceByIdAsync(prod.ProductId);
            }
        }
        
        if (stringBuilder.Length > 0)
            throw new KeyNotFoundException(stringBuilder.ToString());
        
        var createdCart = await _cartRepository.CreateAsync(cart, cancellationToken);
        var result = _mapper.Map<CreateCartResult>(createdCart);
        return result;
    }
}