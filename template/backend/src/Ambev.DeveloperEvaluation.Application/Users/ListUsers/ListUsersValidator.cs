using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUsers;

/// <summary>
/// Validator for ListUsersCommand
/// </summary>
public class ListUsersValidator : AbstractValidator<ListUsersCommand>
{
    /// <summary>
    /// Initializes validation rules for ListUsersCommand
    /// </summary>
    public ListUsersValidator()
    {
    }
}
