using Ambev.DeveloperEvaluation.Application.Carts.CancelCart;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.ListCarts;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.CancelCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.DeleteCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.ListCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.UpdateCart;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartsController(IMediator mediator, IMapper mapper, ILogger<CartsController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CartsController> _logger = logger;

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<ListCartsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCarts(
            [FromQuery] int _page = 1,
            [FromQuery] int _size = 10,
            [FromQuery] string? _order = "branch asc",
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[GetCarts] Request received: page={Page} size={Size} order={Order}", _page, _size, _order);
            var request = new ListCartsRequest
            {
                Page = _page,
                Size = _size,
            };
            var validator = new ListCartsRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
            if (!validationResult.IsValid)
            {
                _logger.LogInformation("[GetCarts] Validation failed: {Errors}", validationResult.Errors);
                return BadRequest(validationResult.Errors);
            }
        
            var command = new ListCartsCommand
            {
                Page = _page,
                Size = _size,
                Order = _order
            };

            try
            {
                _logger.LogInformation("[GetCarts] Sending command: {@Command}", command);
                var result = await _mediator.Send(command, cancellationToken);
                var response = _mapper.Map<ListCartsResponse>(result);
                _logger.LogInformation("[GetCarts] Success");
                return Ok(response);
            }
            catch (ValidationException e)
            {
                _logger.LogInformation(e, "[GetCarts] ValidationException: {Message}", e.Message);
                return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "", e.Message));
            }
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetCartResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCart([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetCart] Request received: id={Id}", id);
            var request = new GetCartRequest { Id = id };
            var validator = new GetCartRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogInformation("[GetCart] Validation failed: {Errors}", validationResult.Errors);
                return BadRequest(validationResult.Errors);
            }

            var command = _mapper.Map<GetCartCommand>(request);
            try
            {
                _logger.LogInformation("[GetCart] Sending command: {@Command}", command);
                var result = await _mediator.Send(command, cancellationToken);
                var response = _mapper.Map<GetCartResponse>(result);
                _logger.LogInformation("[GetCart] Success: id={Id}", id);
                return Ok(response);
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogInformation(e, "[GetCart] NotFound: id={Id} message={Message}", id, e.Message);
                return NotFound(ApiResponse.BuildErrorResponse("KeyNotFound", "Cart not found", e.Message));
            }
            catch (InvalidOperationException e)
            {
                _logger.LogInformation(e, "[GetCart] InvalidOperation: id={Id} message={Message}", id, e.Message);
                return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "", e.Message));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateCartResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCartRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[CreateCart] Request received");
            var validator = new CreateCartRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var command = _mapper.Map<CreateCartCommand>(request);
                _logger.LogInformation("[CreateCart] Sending command: {@Command}", command);
                var result = await _mediator.Send(command, cancellationToken);
                var response = _mapper.Map<CreateCartResponse>(result);
                _logger.LogInformation("[CreateCart] Success");
                return Created(string.Empty, response);
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogInformation(e, "[CreateCart] NotFound: message={Message}", e.Message);
                return NotFound(ApiResponse.BuildErrorResponse("KeyNotFound", "User not found", e.Message));
            }
            catch (InvalidOperationException e)
            {
                _logger.LogInformation(e, "[CreateCart] InvalidOperation: message={Message}", e.Message);
                return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "Field already exists",e.Message));
            }
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateCartResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCart([FromRoute] Guid id, [FromBody] UpdateCartRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[UpdateCart] Request received: id={Id}", id);
            request.Id = id;
            var validator = new UpdateCartRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var command = _mapper.Map<UpdateCartCommand>(request);
                _logger.LogInformation("[UpdateCart] Sending command: {@Command}", command);
                var result = await _mediator.Send(command, cancellationToken);
                var response = _mapper.Map<UpdateCartResponse>(result);
                _logger.LogInformation("[UpdateCart] Success: id={Id}", id);
                return Ok(response);
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogInformation(e, "[UpdateCart] NotFound: id={Id} message={Message}", id, e.Message);
                return NotFound(ApiResponse.BuildErrorResponse("KeyNotFound", "Cart not found", e.Message));
            }
            catch (InvalidOperationException e)
            {
                _logger.LogInformation(e, "[UpdateCart] InvalidOperation: id={Id} message={Message}", id, e.Message);
                return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "Field already exists",e.Message));
            }
        }
        
        [HttpPatch("cancel/{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateCartResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelCart([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[CancelCart] Request received: id={Id}", id);
            var request = new CancelCartRequest()
            {
                Id = id
            };
            var validator = new CancelCartRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var command = _mapper.Map<CancelCartCommand>(request);
                _logger.LogInformation("[CancelCart] Sending command: {@Command}", command);
                var result = await _mediator.Send(command, cancellationToken);
                var response = _mapper.Map<CancelCartResponse>(result);
                _logger.LogInformation("[CancelCart] Success: id={Id}", id);
                return Ok(response);
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogInformation(e, "[CancelCart] NotFound: id={Id} message={Message}", id, e.Message);
                return NotFound(ApiResponse.BuildErrorResponse("KeyNotFound", "Cart not found", e.Message));
            }
            catch (InvalidOperationException e)
            {
                _logger.LogInformation(e, "[CancelCart] InvalidOperation: id={Id} message={Message}", id, e.Message);
                return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "Field already exists",e.Message));
            }
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCart([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[DeleteCart] Request received: id={Id}", id);
            var request = new DeleteCartRequest { Id = id };
            var validator = new DeleteCartRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var command = _mapper.Map<DeleteCartCommand>(request.Id);
                _logger.LogInformation("[DeleteCart] Sending command: {@Command}", command);
                await _mediator.Send(command, cancellationToken);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Cart deleted successfully"
                });
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogInformation(e, "[DeleteCart] NotFound: id={Id} message={Message}", id, e.Message);
                return NotFound(ApiResponse.BuildErrorResponse("KeyNotFound", "Cart not found", e.Message));
            }
            catch (InvalidOperationException e)
            {
                _logger.LogInformation(e, "[DeleteCart] InvalidOperation: id={Id} message={Message}", id, e.Message);
                return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "", e.Message));
            }
        }
        
    }
}
