using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProductCategory;
using Ambev.DeveloperEvaluation.Application.Products.ListProductCategory;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProductsCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProductCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

/// <summary>
/// Controller for managing product operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of ProductsController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Retrieves a product by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product details if found</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetProductRequest { Id = id };
        var validator = new GetProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<GetProductCommand>(request);
        try
        {
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(_mapper.Map<GetProductResponse>(response));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ApiResponse.BuildErrorResponse(e.Message));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(ApiResponse.BuildErrorResponse(e.Message));
        }
    }
    
    /// <summary>
    /// Retrieves a product by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product details if found</returns>
    [HttpGet("categories")]
    [ProducesResponseType(typeof(ApiResponseWithData<ListProductCategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListProductCategories(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _mediator.Send(new ListProductCategoryCommand(), cancellationToken);
            return Ok(_mapper.Map<ListProductCategoryResponse>(response).Categories);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ApiResponse.BuildErrorResponse(e.Message));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(ApiResponse.BuildErrorResponse(e.Message));
        }
    }


    /// <summary>
    /// Retrieves a product by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="_page"></param>
    /// <returns>The product details if found</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<ListProductsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int _page = 1,
        [FromQuery] int _size = 10,
        [FromQuery] string? _order = "price desc,title asc",
        CancellationToken cancellationToken = default)
    {
        
        var command = new ListProductsCommand
        {
            Page = _page,
            Size = _size,
            Order = _order
        };

        try
        {
            // Executa a query via Mediator
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(_mapper.Map<ListProductsResponse>(result));
        }
        catch (ValidationException e)
        {
            return BadRequest(ApiResponse.BuildErrorResponse(e.Message));
        }
    }

    
    [HttpGet("category/{category}")]
    [ProducesResponseType(typeof(ApiResponseWithData<ListProductsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProductsByCategory(
        [FromRoute] string category, 
        [FromQuery] int _page = 1,
        [FromQuery] int _size = 10,
        [FromQuery] string? _order = "price desc,title asc",
        CancellationToken cancellationToken = default)
    {
        var request = new GetProductCategoryRequest() { Category = category };
        var validator = new GetProductCategoryRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var command = new GetProductCategoryCommand()
        {
            Page = _page,
            Size = _size,
            Order = _order,
            Category = category
        };

        try
        {
            // Executa a query via Mediator
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(_mapper.Map<GetProductCategoryResponse>(result));
        }
        catch (ValidationException e)
        {
            return BadRequest(ApiResponse.BuildErrorResponse(e.Message));
        }
    }


    
    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="request">The product creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created product details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateProductResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        try
        {
            var command = _mapper.Map<CreateProductCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, _mapper.Map<CreateProductResponse>(response));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(ApiResponse.BuildErrorResponse(e.Message));
        }
    }
    
    
    /// <summary>
    /// Deletes a product by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the product was deleted</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteProductRequest { Id = id };
        var validator = new DeleteProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        try
        {
            var command = _mapper.Map<DeleteProductCommand>(request.Id);
            await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Product deleted successfully"
            });
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ApiResponse.BuildErrorResponse(e.Message));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(ApiResponse.BuildErrorResponse(e.Message));
        }
    }

    /// <summary>
    /// Updates a existed product
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request">The product creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created product details</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateProductResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        request.Id = id;
        var validator = new UpdateProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        try
        {
            var command = _mapper.Map<UpdateProductCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(_mapper.Map<UpdateProductResponse>(response));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ApiResponse.BuildErrorResponse(e.Message));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(ApiResponse.BuildErrorResponse(e.Message));
        }
    }

   


   
}