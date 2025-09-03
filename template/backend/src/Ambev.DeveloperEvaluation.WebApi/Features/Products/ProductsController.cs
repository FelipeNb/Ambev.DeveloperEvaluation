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
    private readonly ILogger<ProductsController> _logger;

    /// <summary>
    /// Initializes a new instance of ProductsController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public ProductsController(IMediator mediator, IMapper mapper, ILogger<ProductsController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
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
        _logger.LogInformation("[GetProduct] Request received: id={Id}", id);
        var request = new GetProductRequest { Id = id };
        var validator = new GetProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogInformation("[GetProduct] Validation failed: {Errors}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<GetProductCommand>(request);
        try
        {
            _logger.LogInformation("[GetProduct] Sending command: {@Command}", command);
            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<GetProductResponse>(result);
            _logger.LogInformation("[GetProduct] Success: id={Id}", id);
            return Ok(response);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogInformation(e, "[GetProduct] NotFound: id={Id} message={Message}", id, e.Message);
            return NotFound(ApiResponse.BuildErrorResponse("KeyNotFound", "Product not found", e.Message));
        }
        catch (InvalidOperationException e)
        {
            _logger.LogInformation(e, "[GetProduct] InvalidOperation: id={Id} message={Message}", id, e.Message);
            return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "", e.Message));
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
        _logger.LogInformation("[ListProductCategories] Request received");
        try
        {
            var command = new ListProductCategoryCommand();
            _logger.LogInformation("[ListProductCategories] Sending command: {@Command}", command);
            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<ListProductCategoryResponse>(result).Categories;
            _logger.LogInformation("[ListProductCategories] Success count={Count}", response?.Count);
            return Ok(response);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogInformation(e, "[ListProductCategories] NotFound: message={Message}", e.Message);
            return NotFound(ApiResponse.BuildErrorResponse("KeyNotFound", "Product not found", e.Message));
        }
        catch (InvalidOperationException e)
        {
            _logger.LogInformation(e, "[ListProductCategories] InvalidOperation: message={Message}", e.Message);
            return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "", e.Message));
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
        _logger.LogInformation("[GetProducts] Request received: page={Page} size={Size} order={Order}", _page, _size, _order);
        var request = new ListProductsRequest
        {
            Page = _page,
            Size = _size,
        };
        var validator = new ListProductsRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            _logger.LogInformation("[GetProducts] Validation failed: {Errors}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }
        
        var command = new ListProductsCommand
        {
            Page = _page,
            Size = _size,
            Order = _order
        };
        
        

        try
        {
            _logger.LogInformation("[GetProducts] Sending command: {@Command}", command);
            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<ListProductsResponse>(result);
            _logger.LogInformation("[GetProducts] Success: count={Count}", response?.TotalItems);
            return Ok(response);
        }
        catch (ValidationException e)
        {
            _logger.LogInformation(e, "[GetProducts] ValidationException: message={Message}", e.Message);
            return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "", e.Message));
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
        _logger.LogInformation("[GetProductsByCategory] Request received: category={Category} page={Page} size={Size} order={Order}", category, _page, _size, _order);
        var request = new GetProductCategoryRequest() { Category = category };
        var validator = new GetProductCategoryRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogInformation("[GetProductsByCategory] Validation failed: {Errors}", validationResult.Errors);
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
            _logger.LogInformation("[GetProductsByCategory] Sending command: {@Command}", command);
            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<GetProductCategoryResponse>(result);
            _logger.LogInformation("[GetProductsByCategory] Success: category={Category}", category);
            return Ok(response);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogInformation(e, "[GetProductsByCategory] NotFound: category={Category} message={Message}", category, e.Message);
            return NotFound(ApiResponse.BuildErrorResponse("KeyNotFound", "Category not found", e.Message));
        }
        catch (ValidationException e)
        {
            _logger.LogInformation(e, "[GetProductsByCategory] ValidationException: message={Message}", e.Message);
            return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "", e.Message));
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
        _logger.LogInformation("[CreateProduct] Request received");
        var validator = new CreateProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        try
        {
            var command = _mapper.Map<CreateProductCommand>(request);
            _logger.LogInformation("[CreateProduct] Sending command: {@Command}", command);
            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<CreateProductResponse>(result);
            _logger.LogInformation("[CreateProduct] Success");
            return Created(string.Empty, response);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogInformation(e, "[CreateProduct] InvalidOperation: message={Message}", e.Message);
            return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "Field already exists", e.Message));
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
        _logger.LogInformation("[DeleteProduct] Request received: id={Id}", id);
        var request = new DeleteProductRequest { Id = id };
        var validator = new DeleteProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        try
        {
            var command = _mapper.Map<DeleteProductCommand>(request.Id);
            _logger.LogInformation("[DeleteProduct] Sending command: {@Command}", command);
            await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Product deleted successfully"
            });
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogInformation(e, "[DeleteProduct] NotFound: id={Id} message={Message}", id, e.Message);
            return NotFound(ApiResponse.BuildErrorResponse("KeyNotFound", "Product not found", e.Message));
        }
        catch (InvalidOperationException e)
        {
            _logger.LogInformation(e, "[DeleteProduct] InvalidOperation: id={Id} message={Message}", id, e.Message);
            return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "", e.Message));
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
        _logger.LogInformation("[UpdateProduct] Request received: id={Id}", id);
        request.Id = id;
        var validator = new UpdateProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        try
        {
            var command = _mapper.Map<UpdateProductCommand>(request);
            _logger.LogInformation("[UpdateProduct] Sending command: {@Command}", command);
            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<UpdateProductResponse>(result);
            _logger.LogInformation("[UpdateProduct] Success: id={Id}", id);
            return Ok(response);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogInformation(e, "[UpdateProduct] NotFound: id={Id} message={Message}", id, e.Message);
            return NotFound(ApiResponse.BuildErrorResponse("KeyNotFound", "Product not found", e.Message));
        }
        catch (InvalidOperationException e)
        {
            _logger.LogInformation(e, "[UpdateProduct] InvalidOperation: id={Id} message={Message}", id, e.Message);
            return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "Field already exists",e.Message));
        }
    }

   


   
}