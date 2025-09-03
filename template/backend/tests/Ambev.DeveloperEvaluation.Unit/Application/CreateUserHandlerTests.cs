using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateUserHandler"/> class.
/// </summary>
public class CreateUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly CreateUserHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _mapper = Substitute.For<IMapper>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _handler = new CreateUserHandler(_userRepository, _mapper, _passwordHasher);
    }

    /// <summary>
    /// Tests that a valid user creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid user data When creating user Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = command.Username,
            Password = command.Password,
            Email = command.Email,
            Phone = command.Phone,
            Status = command.Status,
            Role = command.Role,
            FirstName = command.FirstName,
            LastName = command.LastName,
            City = command.City,
            Street = command.Street,
            Number = command.Number,
            Zipcode = command.Zipcode,
            Latitude = command.Latitude,
            Longitude = command.Longitude
        };

        var result = new CreateUserResult
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            Status = user.Status,
            Role = user.Role,
            FirstName = user.FirstName,
            LastName = user.LastName,
            City = user.City,
            Street = user.Street,
            Number = user.Number,
            Zipcode = user.Zipcode,
            Latitude = user.Latitude,
            Longitude = user.Longitude
        };

        _mapper.Map<User>(command).Returns(user);
        _mapper.Map<CreateUserResult>(user).Returns(result);

        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.HashPassword(Arg.Any<string>()).Returns("hashedPassword");

        // When
        var createUserResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createUserResult.Should().NotBeNull();
        createUserResult.Id.Should().Be(user.Id);
        createUserResult.Username.Should().Be(user.Username);
        createUserResult.Email.Should().Be(user.Email);
        createUserResult.Phone.Should().Be(user.Phone);
        createUserResult.Status.Should().Be(user.Status);
        createUserResult.Role.Should().Be(user.Role);
        createUserResult.FirstName.Should().Be(user.FirstName);
        createUserResult.LastName.Should().Be(user.LastName);
        createUserResult.City.Should().Be(user.City);
        createUserResult.Street.Should().Be(user.Street);
        createUserResult.Number.Should().Be(user.Number);
        createUserResult.Zipcode.Should().Be(user.Zipcode);
        createUserResult.Latitude.Should().Be(user.Latitude);
        createUserResult.Longitude.Should().Be(user.Longitude);
        await _userRepository.Received(1).CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid user creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid user data When creating user Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateUserCommand(); // Empty command will fail validation

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that the password is hashed before saving the user.
    /// </summary>
    [Fact(DisplayName = "Given user creation request When handling Then password is hashed")]
    public async Task Handle_ValidRequest_HashesPassword()
    {
        // Given
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        var originalPassword = command.Password;
        const string hashedPassword = "h@shedPassw0rd";
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = command.Username,
            Password = command.Password,
            Email = command.Email,
            Phone = command.Phone,
            Status = command.Status,
            Role = command.Role,
            FirstName = command.FirstName,
            LastName = command.LastName,
            City = command.City,
            Street = command.Street,
            Number = command.Number,
            Zipcode = command.Zipcode,
            Latitude = command.Latitude,
            Longitude = command.Longitude
        };

        _mapper.Map<User>(command).Returns(user);
        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.HashPassword(originalPassword).Returns(hashedPassword);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _passwordHasher.Received(1).HashPassword(originalPassword);
        await _userRepository.Received(1).CreateAsync(
            Arg.Is<User>(u => u.Password == hashedPassword),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the mapper is called with the correct command.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps command to user entity")]
    public async Task Handle_ValidRequest_MapsCommandToUser()
    {
        // Given
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = command.Username,
            Password = command.Password,
            Email = command.Email,
            Phone = command.Phone,
            Status = command.Status,
            Role = command.Role,
            FirstName = command.FirstName,
            LastName = command.LastName,
            City = command.City,
            Street = command.Street,
            Number = command.Number,
            Zipcode = command.Zipcode,
            Latitude = command.Latitude,
            Longitude = command.Longitude
        };

        _mapper.Map<User>(command).Returns(user);
        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.HashPassword(Arg.Any<string>()).Returns("hashedPassword");

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<User>(Arg.Is<CreateUserCommand>(c =>
            c.Username == command.Username &&
            c.Email == command.Email &&
            c.Phone == command.Phone &&
            c.Status == command.Status &&
            c.Role == command.Role &&
            c.FirstName == command.FirstName &&
            c.LastName == command.LastName &&
            c.City == command.City &&
            c.Street == command.Street &&
            c.Number == command.Number &&
            c.Zipcode == command.Zipcode &&
            c.Latitude == command.Latitude &&
            c.Longitude == command.Longitude));
    }

    /// <summary>
    /// Tests that a user creation request with null coordinates is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given user data with null coordinates When creating user Then returns success response")]
    public async Task Handle_ValidRequestWithNullCoordinates_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        command.Latitude = null;
        command.Longitude = null;
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = command.Username,
            Password = command.Password,
            Email = command.Email,
            Phone = command.Phone,
            Status = command.Status,
            Role = command.Role,
            FirstName = command.FirstName,
            LastName = command.LastName,
            City = command.City,
            Street = command.Street,
            Number = command.Number,
            Zipcode = command.Zipcode,
            Latitude = null,
            Longitude = null
        };

        var result = new CreateUserResult
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            Status = user.Status,
            Role = user.Role,
            FirstName = user.FirstName,
            LastName = user.LastName,
            City = user.City,
            Street = user.Street,
            Number = user.Number,
            Zipcode = user.Zipcode,
            Latitude = null,
            Longitude = null
        };

        _mapper.Map<User>(command).Returns(user);
        _mapper.Map<CreateUserResult>(user).Returns(result);

        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.HashPassword(Arg.Any<string>()).Returns("hashedPassword");

        // When
        var createUserResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createUserResult.Should().NotBeNull();
        createUserResult.Id.Should().Be(user.Id);
        createUserResult.Latitude.Should().BeNull();
        createUserResult.Longitude.Should().BeNull();
        await _userRepository.Received(1).CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that a user creation request with invalid address data throws validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid address data When creating user Then throws validation exception")]
    public async Task Handle_InvalidAddressData_ThrowsValidationException()
    {
        // Given
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        command.FirstName = ""; // Invalid: empty
        command.City = ""; // Invalid: empty
        command.Number = 0; // Invalid: must be greater than 0

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}
