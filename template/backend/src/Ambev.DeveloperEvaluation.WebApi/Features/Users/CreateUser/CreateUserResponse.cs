using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

/// <summary>
/// API response model for CreateUser operation
/// </summary>

public class CreateUserNameResponse
{
    /// <summary>
    /// The user's first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// The user's last name
    /// </summary>
    public string LastName { get; set; } = string.Empty;
}

public class CreateUserGeoLocationResponse
{
    /// <summary>
    /// The latitude coordinate of the user's location
    /// </summary>
    public string? Lat { get; set; }

    /// <summary>
    /// The longitude coordinate of the user's location
    /// </summary>
    public string? Long { get; set; }
}

public class CreateUserAddressResponse
{
    /// <summary>
    /// The city where the user lives
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// The street where the user lives
    /// </summary>
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// The house/building number
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// The zipcode of the user's address
    /// </summary>
    public string Zipcode { get; set; } = string.Empty;
    public CreateUserGeoLocationResponse GeoLocation { get; set; } = new ();

   
}

/// <summary>
/// API response model for CreateUser operation
/// </summary>
public class CreateUserResponse
{
    /// <summary>
    /// The unique identifier of the user
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The user's full name
    /// </summary>
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// The user's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    public CreateUserNameResponse Name { get; set; } = new();
    public CreateUserAddressResponse Address { get; set; } = new();

    /// <summary>
    /// The user's phone number
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// The user's role in the system
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    /// The current status of the user
    /// </summary>
    public UserStatus Status { get; set; }
    
}
