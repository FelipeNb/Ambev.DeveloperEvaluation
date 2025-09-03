namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;

/// <summary>
/// Request model for getting a user by ID
/// </summary>
public class ListUsersRequest
{
    public int Page { get; set; }
    public int Size { get; set; }
}
