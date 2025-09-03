using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Application.Users.ListUsers;
using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.DeleteUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users;

/// <summary>
/// Controller for managing user operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<UsersController> _logger;

    /// <summary>
    /// Initializes a new instance of UsersController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public UsersController(IMediator mediator, IMapper mapper, ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }
    
    /// <summary>
    /// Retrieves a user by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the user</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user details if found</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[GetUser] Request received: id={Id}", id);
        var request = new GetUserRequest { Id = id };
        var validator = new GetUserRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogInformation("[GetUser] Validation failed: {Errors}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<GetUserCommand>(request);
        try
        {
            _logger.LogInformation("[GetUser] Sending command: {@Command}", command);
            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<GetUserResponse>(result);
            _logger.LogInformation("[GetUser] Success: id={Id}", id);
            return Ok(response);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogInformation(e, "[GetUser] NotFound: id={Id} message={Message}", id, e.Message);
            return NotFound(ApiResponse.BuildErrorResponse("KeyNotFound", "User not found", e.Message));
        }
        catch (InvalidOperationException e)
        {
            _logger.LogInformation(e, "[GetUser] InvalidOperation: id={Id} message={Message}", id, e.Message);
            return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "", e.Message));
        }
    }


    /// <summary>
    /// Retrieves a user by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the user</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="_page"></param>
    /// <returns>The user details if found</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<ListUsersResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUsers(
        [FromQuery] int _page = 1,
        [FromQuery] int _size = 10,
        [FromQuery] string? _order = "username asc,email desc",
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("[GetUsers] Request received: page={Page} size={Size} order={Order}", _page, _size, _order);
        var request = new ListUsersRequest
        {
            Page = _page,
            Size = _size,
        };
        var validator = new ListUsersRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            _logger.LogInformation("[GetUsers] Validation failed: {Errors}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }
        
        var command = new ListUsersCommand
        {
            Page = _page,
            Size = _size,
            Order = _order
        };

        try
        {
            _logger.LogInformation("[GetUsers] Sending command: {@Command}", command);
            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<ListUsersResponse>(result);
            _logger.LogInformation("[GetUsers] Success");
            return Ok(response);
        }
        catch (ValidationException e)
        {
            _logger.LogInformation(e, "[GetUsers] ValidationException: message={Message}", e.Message);
            return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "", e.Message));
        }
    }


    
    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="request">The user creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created user details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateUserResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[CreateUser] Request received");
        var validator = new CreateUserRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        try
        {
            var command = _mapper.Map<CreateUserCommand>(request);
            _logger.LogInformation("[CreateUser] Sending command: {@Command}", command);
            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<CreateUserResponse>(result);
            _logger.LogInformation("[CreateUser] Success");
            return Created(string.Empty, response);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogInformation(e, "[CreateUser] InvalidOperation: message={Message}", e.Message);
            return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "Field already exists",e.Message));
        }
    }
    
    
    /// <summary>
    /// Deletes a user by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the user was deleted</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[DeleteUser] Request received: id={Id}", id);
        var request = new DeleteUserRequest { Id = id };
        var validator = new DeleteUserRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        try
        {
            var command = _mapper.Map<DeleteUserCommand>(request.Id);
            _logger.LogInformation("[DeleteUser] Sending command: {@Command}", command);
            await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "User deleted successfully"
            });
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogInformation(e, "[DeleteUser] NotFound: id={Id} message={Message}", id, e.Message);
            return NotFound(ApiResponse.BuildErrorResponse("KeyNotFound", "User not found", e.Message));
        }
        catch (InvalidOperationException e)
        {
            _logger.LogInformation(e, "[DeleteUser] InvalidOperation: id={Id} message={Message}", id, e.Message);
            return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "", e.Message));
        }
    }

    /// <summary>
    /// Updates a existed user
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request">The user creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created user details</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateUserResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[UpdateUser] Request received: id={Id}", id);
        request.Id = id;
        var validator = new UpdateUserRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        try
        {
            var command = _mapper.Map<UpdateUserCommand>(request);
            _logger.LogInformation("[UpdateUser] Sending command: {@Command}", command);
            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<UpdateUserResponse>(result);
            _logger.LogInformation("[UpdateUser] Success: id={Id}", id);
            return Ok(response);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogInformation(e, "[UpdateUser] NotFound: id={Id} message={Message}", id, e.Message);
            return NotFound(ApiResponse.BuildErrorResponse("KeyNotFound", "User not found", e.Message));
        }
        catch (InvalidOperationException e)
        {
            _logger.LogInformation(e, "[UpdateUser] InvalidOperation: id={Id} message={Message}", id, e.Message);
            return BadRequest(ApiResponse.BuildErrorResponse("InvalidOperation", "Field already exists",e.Message));
        }
    }

   


   
}