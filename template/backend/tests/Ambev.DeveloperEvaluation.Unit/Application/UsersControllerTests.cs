using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Application.Users.ListUsers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the UsersController class.
/// Tests cover all CRUD operations, validation scenarios, error handling, and response formats.
/// </summary>
public class UsersControllerTests
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _mapper = Substitute.For<IMapper>();
        ILogger<UsersController> logger = Substitute.For<ILogger<UsersController>>();
        _controller = new UsersController(_mediator, _mapper, logger);
    }

    #region GetUser Tests

    /// <summary>
    /// Tests that GetUser returns Ok result when user is found successfully.
    /// </summary>
    [Fact(DisplayName = "GetUser should return Ok when user is found")]
    public async Task Given_ValidUserId_When_GetUser_Then_ShouldReturnOk()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var getUserResult = new GetUserResult
        {
            Id = userId,
            Username = "testuser",
            Email = "test@example.com",
            Phone = "998877548",
            Status = UserStatus.Active,
            Role = UserRole.Customer,
            FirstName = "John",
            LastName = "Doe",
            City = "São Paulo",
            Street = "Rua das Flores",
            Number = 123,
            Zipcode = "01234-567"
        };

        var getUserResponse = new GetUserResponse
        {
            Id = userId,
            Username = "testuser",
            Email = "test@example.com",
            Phone = "998877548",
            Status = UserStatus.Active,
            Role = UserRole.Customer,
            Name = new GetUserNameResponse { FirstName = "John", LastName = "Doe" },
            Address = new GetUserAddressResponse
            {
                City = "São Paulo",
                Street = "Rua das Flores",
                Number = 123,
                Zipcode = "01234-567"
            }
        };

        _mapper.Map<GetUserCommand>(Arg.Any<GetUserRequest>()).Returns(new GetUserCommand(userId));
        _mediator.Send(Arg.Any<GetUserCommand>(), Arg.Any<CancellationToken>()).Returns(getUserResult);
        _mapper.Map<GetUserResponse>(getUserResult).Returns(getUserResponse);

        // Act
        var result = await _controller.GetUser(userId, CancellationToken.None);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponseWithData<GetUserResponse>>().Subject;
        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.Id.Should().Be(userId);
        response.Data.Username.Should().Be("testuser");
    }

    /// <summary>
    /// Tests that GetUser returns BadRequest when validation fails.
    /// </summary>
    [Fact(DisplayName = "GetUser should return BadRequest when validation fails")]
    public async Task Given_InvalidUserId_When_GetUser_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var userId = Guid.Empty; // Invalid ID

        // Act
        var result = await _controller.GetUser(userId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    /// <summary>
    /// Tests that GetUser returns BadRequest when InvalidOperationException occurs.
    /// </summary>
    [Fact(DisplayName = "GetUser should return BadRequest when InvalidOperationException occurs")]
    public async Task Given_InvalidOperationException_When_GetUser_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mapper.Map<GetUserCommand>(Arg.Any<GetUserRequest>()).Returns(new GetUserCommand(userId));
        _mediator.Send(Arg.Any<GetUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<GetUserResult>(new InvalidOperationException("Invalid operation")));

        // Act
        var result = await _controller.GetUser(userId, CancellationToken.None);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        response.Success.Should().BeFalse();
        response.Message.Should().Be("Invalid operation");
    }

    #endregion

    #region GetUsers (List Users) Tests

    /// <summary>
    /// Tests that GetUsers returns Ok result with paginated data.
    /// </summary>
    [Fact(DisplayName = "GetUsers should return Ok with paginated data")]
    public async Task Given_ValidPaginationParameters_When_GetUsers_Then_ShouldReturnOk()
    {
        // Arrange
        var listUsersResult = new ListUsersResult
        {
            Items = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "user1",
                    Email = "user1@example.com",
                    Status = UserStatus.Active,
                    Role = UserRole.Customer
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "user2",
                    Email = "user2@example.com",
                    Status = UserStatus.Active,
                    Role = UserRole.Manager
                }
            },
            TotalItems = 2,
            CurrentPage = 1,
            TotalPages = 1
        };

        _mediator.Send(Arg.Any<ListUsersCommand>(), Arg.Any<CancellationToken>()).Returns(listUsersResult);

        // Act
        var result = await _controller.GetUsers(1, 10, "username asc", CancellationToken.None);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponseWithData<ListUsersResponse>>().Subject;
        response.Data?.Items.Should().HaveCount(2);
        response.Data?.TotalItems.Should().Be(2);
        response.Data?.CurrentPage.Should().Be(1);
    }

    /// <summary>
    /// Tests that GetUsers returns BadRequest when validation fails.
    /// </summary>
    [Fact(DisplayName = "GetUsers should return BadRequest when validation fails")]
    public async Task Given_InvalidPaginationParameters_When_GetUsers_Then_ShouldReturnBadRequest()
    {
        // Arrange
        _mediator.Send(Arg.Any<ListUsersCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<ListUsersResult>(new ValidationException("Invalid parameters")));

        // Act
        var result = await _controller.GetUsers(1, 5, "username asc", CancellationToken.None);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        response.Errors.Should().Contain(s => s.Detail == "Invalid parameters");
    }
    
    #endregion

    #region CreateUser Tests

    /// <summary>
    /// Tests that CreateUser returns Created result when user is created successfully.
    /// </summary>
    [Fact(DisplayName = "CreateUser should return Created when user is created successfully")]
    public async Task Given_ValidUserRequest_When_CreateUser_Then_ShouldReturnCreated()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest
        {
            Username = "newuser",
            Password = "SecurePass123!",
            Email = "newuser@example.com",
            Phone = "998877548",
            Status = UserStatus.Active,
            Role = UserRole.Customer,
            Name = new CreateUserNameRequest { FirstName = "John", LastName = "Doe" },
            Address = new CreateUserAddressRequest
            {
                City = "São Paulo",
                Street = "Rua das Flores",
                Number = 123,
                Zipcode = "01234-567"
            }
        };

        var createUserResult = new CreateUserResult
        {
            Id = Guid.NewGuid(),
            Username = "newuser",
            Email = "newuser@example.com",
            Status = UserStatus.Active,
            Role = UserRole.Customer
        };

        var createUserResponse = new CreateUserResponse
        {
            Id = createUserResult.Id,
            Username = "newuser",
            Email = "newuser@example.com",
            Status = UserStatus.Active,
            Role = UserRole.Customer
        };

        _mapper.Map<CreateUserCommand>(createUserRequest).Returns(new CreateUserCommand());
        _mediator.Send(Arg.Any<CreateUserCommand>(), Arg.Any<CancellationToken>()).Returns(createUserResult);
        _mapper.Map<CreateUserResponse>(createUserResult).Returns(createUserResponse);

        // Act
        var result = await _controller.CreateUser(createUserRequest, CancellationToken.None);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedResult>().Subject;
        var response = createdResult.Value.Should().BeOfType<CreateUserResponse>().Subject;
        response.Id.Should().Be(createUserResult.Id);
        response.Username.Should().Be("newuser");
        response.Email.Should().Be("newuser@example.com");
    }

    /// <summary>
    /// Tests that CreateUser returns BadRequest when validation fails.
    /// </summary>
    [Fact(DisplayName = "CreateUser should return BadRequest when validation fails")]
    public async Task Given_InvalidUserRequest_When_CreateUser_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest
        {
            Username = "", // Invalid: empty
            Email = "invalid-email", // Invalid: not a valid email
            Password = "weak", // Invalid: doesn't meet requirements
            Phone = "invalid-phone", // Invalid: doesn't match pattern
            Status = UserStatus.Unknown, // Invalid: cannot be Unknown
            Role = UserRole.None, // Invalid: cannot be None
            Name = new CreateUserNameRequest { FirstName = "", LastName = "" }, // Invalid: empty
            Address = new CreateUserAddressRequest
            {
                City = "", // Invalid: empty
                Street = "", // Invalid: empty
                Number = 0, // Invalid: must be greater than 0
                Zipcode = "" // Invalid: empty
            }
        };

        // Act
        var result = await _controller.CreateUser(createUserRequest, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    /// <summary>
    /// Tests that CreateUser returns BadRequest when InvalidOperationException occurs.
    /// </summary>
    [Fact(DisplayName = "CreateUser should return BadRequest when InvalidOperationException occurs")]
    public async Task Given_InvalidOperationException_When_CreateUser_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest
        {
            Username = "newuser",
            Password = "SecurePass123!",
            Email = "newuser@example.com",
            Phone = "956587458",
            Status = UserStatus.Active,
            Role = UserRole.Customer,
            Name = new CreateUserNameRequest { FirstName = "John", LastName = "Doe" },
            Address = new CreateUserAddressRequest
            {
                City = "São Paulo",
                Street = "Rua das Flores",
                Number = 123,
                Zipcode = "01234-567"
            }
        };

        _mapper.Map<CreateUserCommand>(createUserRequest).Returns(new CreateUserCommand());
        _mediator.Send(Arg.Any<CreateUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<CreateUserResult>(new InvalidOperationException("User already exists")));

        // Act
        var result = await _controller.CreateUser(createUserRequest, CancellationToken.None);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        response.Success.Should().BeFalse();
        response.Errors.Any(s => s.Detail == "User already exists").Should().BeTrue();
    }

    #endregion

    #region DeleteUser Tests
    
    /// <summary>
    /// Tests that DeleteUser returns BadRequest when validation fails.
    /// </summary>
    [Fact(DisplayName = "DeleteUser should return BadRequest when validation fails")]
    public async Task Given_InvalidUserId_When_DeleteUser_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var userId = Guid.Empty; // Invalid ID

        // Act
        var result = await _controller.DeleteUser(userId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    #endregion

    #region UpdateUser Tests
    

    /// <summary>
    /// Tests that UpdateUser returns BadRequest when validation fails.
    /// </summary>
    [Fact(DisplayName = "UpdateUser should return BadRequest when validation fails")]
    public async Task Given_InvalidUpdateRequest_When_UpdateUser_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var updateUserRequest = new UpdateUserRequest
        {
            Username = "", // Invalid: empty
            Email = "invalid-email", // Invalid: not a valid email
            Password = "weak", // Invalid: doesn't meet requirements
            Phone = "invalid-phone", // Invalid: doesn't match pattern
            Status = UserStatus.Unknown, // Invalid: cannot be Unknown
            Role = UserRole.None, // Invalid: cannot be None
            Name = new UpdateUserNameRequest { FirstName = "", LastName = "" }, // Invalid: empty
            Address = new UpdateUserAddressRequest
            {
                City = "", // Invalid: empty
                Street = "", // Invalid: empty
                Number = 0, // Invalid: must be greater than 0
                Zipcode = "" // Invalid: empty
            }
        };

        // Act
        var result = await _controller.UpdateUser(userId, updateUserRequest, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }


    #endregion

    #region Pagination and Sorting Tests

    /// <summary>
    /// Tests that GetUsers handles different page sizes correctly.
    /// </summary>
    [Theory(DisplayName = "GetUsers should handle different page sizes correctly")]
    [InlineData(1, 5)]
    [InlineData(2, 10)]
    [InlineData(3, 20)]
    public async Task Given_DifferentPageSizes_When_GetUsers_Then_ShouldHandleCorrectly(int page, int size)
    {
        // Arrange
        var listUsersResult = new ListUsersResult
        {
            Items = new List<User>(),
            TotalItems = 0,
            CurrentPage = page,
            TotalPages = 0
        };

        _mediator.Send(Arg.Any<ListUsersCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(listUsersResult));

        _mapper.Map<ListUsersResponse>(Arg.Any<ListUsersResult>())
            .Returns(new ListUsersResponse
            {
                Items = new List<GetUserResponse>(),
                TotalItems = 0,
                CurrentPage = page,
                TotalPages = 0
            });

        // Act
        var result = await _controller.GetUsers(page, size, "username asc", CancellationToken.None);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponseWithData<ListUsersResponse>>().Subject;
        response?.Data?.CurrentPage.Should().Be(page);

    }

    /// <summary>
    /// Tests that GetUsers handles different sorting orders correctly.
    /// </summary>
    [Theory(DisplayName = "GetUsers should handle different sorting orders correctly")]
    [InlineData("username asc")]
    [InlineData("email desc")]
    [InlineData("created_at asc")]
    [InlineData("status desc, username asc")]
    public async Task Given_DifferentSortOrders_When_GetUsers_Then_ShouldHandleCorrectly(string order)
    {
        // Arrange
        var listUsersResult = new ListUsersResult
        {
            Items = new List<User>(),
            TotalItems = 0,
            CurrentPage = 1,
            TotalPages = 0
        };

        _mediator.Send(Arg.Any<ListUsersCommand>(), Arg.Any<CancellationToken>()).Returns(listUsersResult);

        // Act
        var result = await _controller.GetUsers(1, 10, order, CancellationToken.None);

        _mapper.Map<ListUsersResponse>(Arg.Any<ListUsersResult>())
            .Returns(new ListUsersResponse
            {
                Items = new List<GetUserResponse>(),
                TotalItems = 0,
                CurrentPage = 1,
                TotalPages = 0
            });
        
        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponseWithData<ListUsersResponse>>().Subject;
        response.Should().NotBeNull();
    }

    #endregion

}