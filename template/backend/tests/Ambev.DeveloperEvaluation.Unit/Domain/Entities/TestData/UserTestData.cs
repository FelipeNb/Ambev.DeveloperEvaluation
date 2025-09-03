using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class UserTestData
{
    /// <summary>
    /// Configures the Faker to generate valid User entities.
    /// The generated users will have valid:
    /// - Username (using internet usernames)
    /// - Password (meeting complexity requirements)
    /// - Email (valid format)
    /// - Phone (Brazilian format)
    /// - Status (Active or Suspended)
    /// - Role (Customer or Admin)
    /// - Name and address information
    /// </summary>
    private static readonly Faker<User> UserFaker = new Faker<User>()
        .RuleFor(u => u.Username, f => f.Internet.UserName())
        .RuleFor(u => u.Password, f => $"Test@{f.Random.Number(100, 999)}")
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
        .RuleFor(u => u.Status, f => f.PickRandom(UserStatus.Active, UserStatus.Suspended))
        .RuleFor(u => u.Role, f => f.PickRandom(UserRole.Customer, UserRole.Admin))
        .RuleFor(u => u.FirstName, f => f.Name.FirstName())
        .RuleFor(u => u.LastName, f => f.Name.LastName())
        .RuleFor(u => u.City, f => f.Address.City())
        .RuleFor(u => u.Street, f => f.Address.StreetName())
        .RuleFor(u => u.Number, f => f.Random.Number(1, 9999))
        .RuleFor(u => u.Zipcode, f => f.Address.ZipCode("#####-###"))
        .RuleFor(u => u.Latitude, f => f.Random.Bool() ? f.Address.Latitude().ToString("F4") : null)
        .RuleFor(u => u.Longitude, f => f.Random.Bool() ? f.Address.Longitude().ToString("F4") : null);

    /// <summary>
    /// Generates a valid User entity with randomized data.
    /// The generated user will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid User entity with randomly generated data.</returns>
    public static User GenerateValidUser()
    {
        return UserFaker.Generate();
    }

    /// <summary>
    /// Generates a valid email address using Faker.
    /// The generated email will:
    /// - Follow the standard email format (user@domain.com)
    /// - Have valid characters in both local and domain parts
    /// - Have a valid TLD
    /// </summary>
    /// <returns>A valid email address.</returns>
    public static string GenerateValidEmail()
    {
        return new Faker().Internet.Email();
    }

    /// <summary>
    /// Generates a valid password that meets all complexity requirements.
    /// The generated password will have:
    /// - At least 8 characters
    /// - At least one uppercase letter
    /// - At least one lowercase letter
    /// - At least one number
    /// - At least one special character
    /// </summary>
    /// <returns>A valid password meeting all complexity requirements.</returns>
    public static string GenerateValidPassword()
    {
        return $"Test@{new Faker().Random.Number(100, 999)}";
    }

    /// <summary>
    /// Generates a valid Brazilian phone number.
    /// The generated phone number will:
    /// - Start with country code (+55)
    /// - Have a valid area code (11-99)
    /// - Have 9 digits for the phone number
    /// - Follow the format: +55XXXXXXXXXXXX
    /// </summary>
    /// <returns>A valid Brazilian phone number.</returns>
    public static string GenerateValidPhone()
    {
        var faker = new Faker();
        return $"+55{faker.Random.Number(11, 99)}{faker.Random.Number(100000000, 999999999)}";
    }

    /// <summary>
    /// Generates a valid username.
    /// The generated username will:
    /// - Be between 3 and 50 characters
    /// - Use internet username conventions
    /// - Contain only valid characters
    /// </summary>
    /// <returns>A valid username.</returns>
    public static string GenerateValidUsername()
    {
        return new Faker().Internet.UserName();
    }

    /// <summary>
    /// Generates a valid first name.
    /// The generated first name will:
    /// - Be between 1 and 50 characters
    /// - Use realistic first names
    /// </summary>
    /// <returns>A valid first name.</returns>
    public static string GenerateValidFirstName()
    {
        return new Faker().Name.FirstName();
    }

    /// <summary>
    /// Generates a valid last name.
    /// The generated last name will:
    /// - Be between 1 and 50 characters
    /// - Use realistic last names
    /// </summary>
    /// <returns>A valid last name.</returns>
    public static string GenerateValidLastName()
    {
        return new Faker().Name.LastName();
    }

    /// <summary>
    /// Generates a valid city name.
    /// The generated city will:
    /// - Be between 1 and 100 characters
    /// - Use realistic city names
    /// </summary>
    /// <returns>A valid city name.</returns>
    public static string GenerateValidCity()
    {
        return new Faker().Address.City();
    }

    /// <summary>
    /// Generates a valid street name.
    /// The generated street will:
    /// - Be between 1 and 100 characters
    /// - Use realistic street names
    /// </summary>
    /// <returns>A valid street name.</returns>
    public static string GenerateValidStreet()
    {
        return new Faker().Address.StreetName();
    }

    /// <summary>
    /// Generates a valid house/building number.
    /// The generated number will:
    /// - Be greater than 0
    /// - Be a realistic house number
    /// </summary>
    /// <returns>A valid house/building number.</returns>
    public static int GenerateValidNumber()
    {
        return new Faker().Random.Number(1, 9999);
    }

    /// <summary>
    /// Generates a valid zipcode.
    /// The generated zipcode will:
    /// - Be between 1 and 20 characters
    /// - Follow Brazilian zipcode format
    /// </summary>
    /// <returns>A valid zipcode.</returns>
    public static string GenerateValidZipcode()
    {
        return new Faker().Address.ZipCode("#####-###");
    }

    /// <summary>
    /// Generates a valid latitude coordinate.
    /// The generated latitude will:
    /// - Be between 1 and 20 characters
    /// - Be a realistic latitude value
    /// </summary>
    /// <returns>A valid latitude coordinate.</returns>
    public static string GenerateValidLatitude()
    {
        return new Faker().Address.Latitude().ToString("F4");
    }

    /// <summary>
    /// Generates a valid longitude coordinate.
    /// The generated longitude will:
    /// - Be between 1 and 20 characters
    /// - Be a realistic longitude value
    /// </summary>
    /// <returns>A valid longitude coordinate.</returns>
    public static string GenerateValidLongitude()
    {
        return new Faker().Address.Longitude().ToString("F4");
    }

    /// <summary>
    /// Generates an invalid email address for testing negative scenarios.
    /// The generated email will:
    /// - Not follow the standard email format
    /// - Not contain the @ symbol
    /// - Be a simple word or string
    /// This is useful for testing email validation error cases.
    /// </summary>
    /// <returns>An invalid email address.</returns>
    public static string GenerateInvalidEmail()
    {
        var faker = new Faker();
        return faker.Lorem.Word();
    }

    /// <summary>
    /// Generates an invalid password for testing negative scenarios.
    /// The generated password will:
    /// - Not meet the minimum length requirement
    /// - Not contain all required character types
    /// This is useful for testing password validation error cases.
    /// </summary>
    /// <returns>An invalid password.</returns>
    public static string GenerateInvalidPassword()
    {
        return new Faker().Lorem.Word();
    }

    /// <summary>
    /// Generates an invalid phone number for testing negative scenarios.
    /// The generated phone number will:
    /// - Not follow the Brazilian phone number format
    /// - Not have the correct length
    /// - Not start with the country code
    /// This is useful for testing phone validation error cases.
    /// </summary>
    /// <returns>An invalid phone number.</returns>
    public static string GenerateInvalidPhone()
    {
        return new Faker().Random.AlphaNumeric(5);
    }

    /// <summary>
    /// Generates a username that exceeds the maximum length limit.
    /// The generated username will:
    /// - Be longer than 50 characters
    /// - Contain random alphanumeric characters
    /// This is useful for testing username length validation error cases.
    /// </summary>
    /// <returns>A username that exceeds the maximum length limit.</returns>
    public static string GenerateLongUsername()
    {
        return new Faker().Random.String2(51);
    }

    /// <summary>
    /// Generates an invalid first name for testing negative scenarios.
    /// The generated first name will be empty.
    /// </summary>
    /// <returns>An invalid first name.</returns>
    public static string GenerateInvalidFirstName()
    {
        return "";
    }

    /// <summary>
    /// Generates an invalid last name for testing negative scenarios.
    /// The generated last name will be empty.
    /// </summary>
    /// <returns>An invalid last name.</returns>
    public static string GenerateInvalidLastName()
    {
        return "";
    }

    /// <summary>
    /// Generates an invalid city for testing negative scenarios.
    /// The generated city will be empty.
    /// </summary>
    /// <returns>An invalid city.</returns>
    public static string GenerateInvalidCity()
    {
        return "";
    }

    /// <summary>
    /// Generates an invalid street for testing negative scenarios.
    /// The generated street will be empty.
    /// </summary>
    /// <returns>An invalid street.</returns>
    public static string GenerateInvalidStreet()
    {
        return "";
    }

    /// <summary>
    /// Generates an invalid number for testing negative scenarios.
    /// The generated number will be 0 or negative.
    /// </summary>
    /// <returns>An invalid number.</returns>
    public static int GenerateInvalidNumber()
    {
        return new Faker().Random.Number(-10, 0);
    }

    /// <summary>
    /// Generates an invalid zipcode for testing negative scenarios.
    /// The generated zipcode will be empty.
    /// </summary>
    /// <returns>An invalid zipcode.</returns>
    public static string GenerateInvalidZipcode()
    {
        return "";
    }
}
