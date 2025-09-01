using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUsers;

/// <summary>
/// Command for retrieving a user by their ID
/// </summary>
public record ListUsersCommand : IRequest<ListUsersResult>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public string? Order { get; set; }
}
