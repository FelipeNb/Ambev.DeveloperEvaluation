using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProductCategory;

/// <summary>
/// Handler for processing ListProductCategoryCommand requests
/// </summary>
public class ListProductCategoryHandler : IRequestHandler<ListProductCategoryCommand, ListProductCategoryResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of ListProductCategoryHandler
    /// </summary>
    /// <param name="productRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public ListProductCategoryHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the ListProductCategoryCommand request
    /// </summary>
    /// <param name="request">The ListProductCategory command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user details if found</returns>
    public async Task<ListProductCategoryResult> Handle(ListProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var validator = new ListProductCategoryValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var product = await _productRepository.GetAllCategoriesAsync(cancellationToken);

        return new ListProductCategoryResult()
        {
            Categories = product
        };
    }
}
