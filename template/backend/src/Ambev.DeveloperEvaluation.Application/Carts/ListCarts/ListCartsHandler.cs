using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

/// <summary>
/// Handler for processing ListCartsCommand requests
/// </summary>
public class ListCartsHandler : IRequestHandler<ListCartsCommand, ListCartsResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of ListCartsHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for ListCartsCommand</param>
    public ListCartsHandler(
        ICartRepository cartRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the ListCartsCommand request
    /// </summary>
    /// <param name="request">The ListCarts command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cart details if found</returns>
    public async Task<ListCartsResult> Handle(ListCartsCommand request, CancellationToken cancellationToken)
    {
        var validator = new ListCartsValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var cart = await _cartRepository.GetAllAsync(request.Page, request.Size,request.Order, cancellationToken);
        
        return _mapper.Map<ListCartsResult>(cart);
    }
}
