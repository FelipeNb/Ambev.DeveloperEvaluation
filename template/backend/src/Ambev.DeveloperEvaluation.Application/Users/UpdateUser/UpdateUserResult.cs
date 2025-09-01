using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// Response model for UpdateUser operation
/// </summary>
public class UpdateUserResult
{
    /// <summary>
    /// The unique identifier of the user
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the username of the updated user.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the updated user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number of the updated user.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the role of the updated user.
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    /// Gets or sets the status of the updated user.
    /// </summary>
    public UserStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the first name of the updated user.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the last name of the updated user.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city where the updated user lives.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the street where the updated user lives.
    /// </summary>
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the house/building number of the updated user.
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Gets or sets the zipcode of the updated user's address.
    /// </summary>
    public string Zipcode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the latitude coordinate of the updated user's location.
    /// </summary>
    public string? Latitude { get; set; }

    /// <summary>
    /// Gets or sets the longitude coordinate of the updated user's location.
    /// </summary>
    public string? Longitude { get; set; }
}
