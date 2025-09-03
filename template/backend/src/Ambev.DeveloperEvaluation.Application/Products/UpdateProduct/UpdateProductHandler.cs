using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Handler for processing UpdateProductCommand requests
/// </summary>
public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UpdateProductHandler
    /// </summary>
    /// <param name="productRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public UpdateProductHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the UpdateProductCommand request
    /// </summary>
    /// <param name="request">The UpdateProduct command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user details if found</returns>
    public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await new UpdateProductValidator().ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var byEmail = await _productRepository.GetByTitleAsync(request.Title, cancellationToken);
        if (byEmail != null && byEmail.Id != request.Id)
            throw new InvalidOperationException($"Product with title {request.Title} already exists");
        
        var existingProduct = await _productRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException("Product does not exist");

        _mapper.Map(request, existingProduct);
        
        await _productRepository.UpdateAsync(existingProduct, cancellationToken);

        return _mapper.Map<UpdateProductResult>(existingProduct);
    }
}
