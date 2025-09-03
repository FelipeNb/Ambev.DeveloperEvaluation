using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductCategory;

/// <summary>
/// Handler for processing GetProductCategoryCommand requests
/// </summary>
public class GetProductCategoryHandler : IRequestHandler<GetProductCategoryCommand, GetProductsCategoryResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetProductCategoryHandler
    /// </summary>
    /// <param name="productRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetProductCategoryHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetProductCategoryCommand request
    /// </summary>
    /// <param name="request">The GetProductCategory command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user details if found</returns>
    public async Task<GetProductsCategoryResult> Handle(GetProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var validator = new GetProductCategoryValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var product = await _productRepository.GetAllByCategoryAsync(request.Category, request.Page, request.Size, request.Order, cancellationToken);

        return _mapper.Map<GetProductsCategoryResult>(product);
    }
}
