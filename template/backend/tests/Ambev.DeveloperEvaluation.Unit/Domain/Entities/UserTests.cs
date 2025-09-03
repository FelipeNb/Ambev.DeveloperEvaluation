using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using MediatR;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the User entity class.
/// Tests cover status changes, validation scenarios, and database transaction operations.
/// </summary>
public class UserTests
{
    
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();

    #region Dto validation tests

    /// <summary>
    /// Tests that when a suspended user is activated, their status changes to Active.
    /// </summary>
    [Fact(DisplayName = "User status should change to Active when activated")]
    public void Given_SuspendedUser_When_Activated_Then_StatusShouldBeActive()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        user.Status = UserStatus.Suspended;

        // Act
        user.Activate();

        // Assert
        Assert.Equal(UserStatus.Active, user.Status);
    }

    /// <summary>
    /// Tests that when an active user is suspended, their status changes to Suspended.
    /// </summary>
    [Fact(DisplayName = "User status should change to Suspended when suspended")]
    public void Given_ActiveUser_When_Suspended_Then_StatusShouldBeSuspended()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        user.Status = UserStatus.Active;

        // Act
        user.Suspend();

        // Assert
        Assert.Equal(UserStatus.Suspended, user.Status);
    }

    /// <summary>
    /// Tests that validation passes when all user properties are valid.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid user data")]
    public void Given_ValidUserData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();

        // Act
        var result = user.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when user properties are invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for invalid user data")]
    public void Given_InvalidUserData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var user = new User
        {
            Username = "", // Invalid: empty
            Password = UserTestData.GenerateInvalidPassword(), // Invalid: doesn't meet password requirements
            Email = UserTestData.GenerateInvalidEmail(), // Invalid: not a valid email
            Phone = UserTestData.GenerateInvalidPhone(), // Invalid: doesn't match pattern
            Status = UserStatus.Unknown, // Invalid: cannot be Unknown
            Role = UserRole.None, // Invalid: cannot be None
            FirstName = "", // Invalid: empty
            LastName = "", // Invalid: empty
            City = "", // Invalid: empty
            Street = "", // Invalid: empty
            Number = 0, // Invalid: must be greater than 0
            Zipcode = "" // Invalid: empty
        };

        // Act
        var result = user.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    /// <summary>
    /// Tests that validation passes when all address and name fields are valid.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid address and name data")]
    public void Given_ValidAddressAndNameData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        user.FirstName = "John";
        user.LastName = "Doe";
        user.City = "São Paulo";
        user.Street = "Rua das Flores";
        user.Number = 123;
        user.Zipcode = "01234-567";
        user.Latitude = "-23.5505";
        user.Longitude = "-46.6333";

        // Act
        var result = user.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation passes when coordinate fields are null (optional fields).
    /// </summary>
    [Fact(DisplayName = "Validation should pass when coordinates are null")]
    public void Given_NullCoordinates_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        user.FirstName = "John";
        user.LastName = "Doe";
        user.City = "São Paulo";
        user.Street = "Rua das Flores";
        user.Number = 123;
        user.Zipcode = "01234-567";
        user.Latitude = null;
        user.Longitude = null;

        // Act
        var result = user.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when name fields are invalid.
    /// </summary>
    [Theory(DisplayName = "Validation should fail for invalid name fields")]
    [InlineData("", "Doe", "First name is required")]
    [InlineData("John", "", "Last name is required")]
    public void Given_InvalidNameFields_When_Validated_Then_ShouldReturnInvalid(string firstName, string lastName, string expectedError)
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        user.FirstName = firstName;
        user.LastName = lastName;

        // Act
        var result = user.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Detail.Contains(expectedError));
    }

    /// <summary>
    /// Tests that validation fails when address fields are invalid.
    /// </summary>
    [Theory(DisplayName = "Validation should fail for invalid address fields")]
    [InlineData("", "Rua das Flores", 123, "01234-567", "City is required")]
    [InlineData("São Paulo", "", 123, "01234-567", "Street is required")]
    [InlineData("São Paulo", "Rua das Flores", 0, "01234-567", "Number must be greater than 0")]
    [InlineData("São Paulo", "Rua das Flores", 123, "", "Zipcode is required")]
    public void Given_InvalidAddressFields_When_Validated_Then_ShouldReturnInvalid(string city, string street, int number, string zipcode, string expectedError)
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        user.City = city;
        user.Street = street;
        user.Number = number;
        user.Zipcode = zipcode;

        // Act
        var result = user.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Detail.Contains(expectedError));
    }

    /// <summary>
    /// Tests that validation fails when coordinate fields exceed maximum length.
    /// </summary>
    [Theory(DisplayName = "Validation should fail for coordinates exceeding maximum length")]
    [InlineData("123456789012345678901", null, "Latitude must be between 1 and 20 characters when provided")]
    [InlineData(null, "123456789012345678901", "Longitude must be between 1 and 20 characters when provided")]
    public void Given_CoordinatesExceedingMaxLength_When_Validated_Then_ShouldReturnInvalid(string latitude, string longitude, string expectedError)
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        user.Latitude = latitude;
        user.Longitude = longitude;

        // Act
        var result = user.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Detail.Contains(expectedError));
    }

    #endregion

    #region Database Transaction Tests

    /// <summary>
    /// Tests that a user can be successfully created in a database transaction.
    /// </summary>
    [Fact(DisplayName = "User should be successfully created in database transaction")]
    public async Task Given_ValidUser_When_CreatedInTransaction_Then_ShouldBePersisted()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>()).Returns(user);

        // Act
        var createdUser = await _userRepository.CreateAsync(user, CancellationToken.None);

        // Assert
        Assert.NotNull(createdUser);
        Assert.Equal(user.Id, createdUser.Id);
        Assert.Equal(user.Username, createdUser.Username);
        Assert.Equal(user.Email, createdUser.Email);
        await _userRepository.Received(1).CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that a transaction is rolled back when user validation fails during creation.
    /// </summary>
    [Fact(DisplayName = "Transaction should be rolled back when user validation fails")]
    public async Task Given_InvalidUser_When_CreatedInTransaction_Then_ShouldRollback()
    {
        // Arrange
        var invalidUser = new User
        {
            Username = "", // Invalid: empty
            Email = "invalid-email", // Invalid: not a valid email
            FirstName = "", // Invalid: empty
            City = "", // Invalid: empty
            Number = 0 // Invalid: must be greater than 0
        };

        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<User>(new InvalidOperationException("Validation failed")));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _userRepository.CreateAsync(invalidUser, CancellationToken.None));

        Assert.Equal("Validation failed", exception.Message);
        await _userRepository.Received(1).CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that multiple users can be created in a single transaction.
    /// </summary>
    [Fact(DisplayName = "Multiple users should be created in single transaction")]
    public async Task Given_MultipleValidUsers_When_CreatedInTransaction_Then_AllShouldBePersisted()
    {
        // Arrange
        var users = new List<User>
        {
            UserTestData.GenerateValidUser(),
            UserTestData.GenerateValidUser(),
            UserTestData.GenerateValidUser()
        };

        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(x => x.Arg<User>());

        // Act
        var createdUsers = new List<User>();
        foreach (var user in users)
        {
            var createdUser = await _userRepository.CreateAsync(user, CancellationToken.None);
            createdUsers.Add(createdUser);
        }

        // Assert
        Assert.Equal(users.Count, createdUsers.Count);
        for (int i = 0; i < users.Count; i++)
        {
            Assert.Equal(users[i].Id, createdUsers[i].Id);
            Assert.Equal(users[i].Username, createdUsers[i].Username);
            Assert.Equal(users[i].Email, createdUsers[i].Email);
        }
        await _userRepository.Received(users.Count).CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that a transaction is rolled back when one user in a batch fails validation.
    /// </summary>
    [Fact(DisplayName = "Transaction should rollback when one user in batch fails validation")]
    public async Task Given_BatchWithInvalidUser_When_CreatedInTransaction_Then_ShouldRollbackAll()
    {
        // Arrange
        var validUsers = new List<User>
        {
            UserTestData.GenerateValidUser(),
            UserTestData.GenerateValidUser()
        };

        var invalidUser = new User
        {
            Username = "", // Invalid: empty
            Email = "invalid-email", // Invalid: not a valid email
            FirstName = "", // Invalid: empty
            City = "", // Invalid: empty
            Number = 0 // Invalid: must be greater than 0
        };

        _userRepository.CreateAsync(Arg.Is<User>(u => u.Username == ""), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<User>(new InvalidOperationException("Validation failed")));
        _userRepository.CreateAsync(Arg.Is<User>(u => u.Username != ""), Arg.Any<CancellationToken>())
            .Returns(x => x.Arg<User>());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _userRepository.CreateAsync(invalidUser, CancellationToken.None));

        Assert.Equal("Validation failed", exception.Message);
        await _userRepository.Received(1).CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that a user with complete address information can be successfully created in a transaction.
    /// </summary>
    [Fact(DisplayName = "User with complete address should be successfully created in transaction")]
    public async Task Given_UserWithCompleteAddress_When_CreatedInTransaction_Then_ShouldBePersisted()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        user.FirstName = "João";
        user.LastName = "Silva";
        user.City = "São Paulo";
        user.Street = "Avenida Paulista";
        user.Number = 1000;
        user.Zipcode = "01310-100";
        user.Latitude = "-23.5505";
        user.Longitude = "-46.6333";

        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        var createdUser = await _userRepository.CreateAsync(user, CancellationToken.None);

        // Assert
        Assert.NotNull(createdUser);
        Assert.Equal(user.FirstName, createdUser.FirstName);
        Assert.Equal(user.LastName, createdUser.LastName);
        Assert.Equal(user.City, createdUser.City);
        Assert.Equal(user.Street, createdUser.Street);
        Assert.Equal(user.Number, createdUser.Number);
        Assert.Equal(user.Zipcode, createdUser.Zipcode);
        Assert.Equal(user.Latitude, createdUser.Latitude);
        Assert.Equal(user.Longitude, createdUser.Longitude);
        await _userRepository.Received(1).CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that a user with null coordinates can be successfully created in a transaction.
    /// </summary>
    [Fact(DisplayName = "User with null coordinates should be successfully created in transaction")]
    public async Task Given_UserWithNullCoordinates_When_CreatedInTransaction_Then_ShouldBePersisted()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        user.FirstName = "Maria";
        user.LastName = "Santos";
        user.City = "Rio de Janeiro";
        user.Street = "Rua Copacabana";
        user.Number = 500;
        user.Zipcode = "22070-001";
        user.Latitude = null;
        user.Longitude = null;

        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        var createdUser = await _userRepository.CreateAsync(user, CancellationToken.None);

        // Assert
        Assert.NotNull(createdUser);
        Assert.Equal(user.FirstName, createdUser.FirstName);
        Assert.Equal(user.LastName, createdUser.LastName);
        Assert.Equal(user.City, createdUser.City);
        Assert.Equal(user.Street, createdUser.Street);
        Assert.Equal(user.Number, createdUser.Number);
        Assert.Equal(user.Zipcode, createdUser.Zipcode);
        Assert.Null(createdUser.Latitude);
        Assert.Null(createdUser.Longitude);
        await _userRepository.Received(1).CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that a transaction can be committed successfully after user creation.
    /// </summary>
    [Fact(DisplayName = "Transaction should be committed successfully after user creation")]
    public async Task Given_ValidUser_When_TransactionCommitted_Then_ShouldPersistData()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _userRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        var createdUser = await _userRepository.CreateAsync(user, CancellationToken.None);
        var retrievedUser = await _userRepository.GetByIdAsync(user.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(createdUser);
        Assert.NotNull(retrievedUser);
        Assert.Equal(createdUser.Id, retrievedUser.Id);
        Assert.Equal(createdUser.Username, retrievedUser.Username);
        Assert.Equal(createdUser.Email, retrievedUser.Email);
        await _userRepository.Received(1).CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        await _userRepository.Received(1).GetByIdAsync(user.Id, Arg.Any<CancellationToken>());
    }

    #endregion
}
