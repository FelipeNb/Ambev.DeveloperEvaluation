using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CancelCart;

/// <summary>
/// Handler for processing CancelCartCommand requests
/// </summary>
public class CancelCartHandler : IRequestHandler<CancelCartCommand, CancelCartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CancelCartHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CancelCartHandler(
        ICartRepository cartRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the CancelCartCommand request
    /// </summary>
    /// <param name="request">The CancelCart command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cart details if found</returns>
    public async Task<CancelCartResult> Handle(CancelCartCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await new CancelCartValidator().ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var cartStored = await _cartRepository.GetByIdAsync(command.Id);
        if(cartStored is null)
            throw new KeyNotFoundException($"Cart was not found with ID {command.Id}");
        
        cartStored.Cancel();
        
        await _cartRepository.CancelAsync(command.Id, cancellationToken);
        var result = _mapper.Map<CancelCartResult>(cartStored);
        return result;
    }
}
