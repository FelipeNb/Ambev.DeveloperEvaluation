using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUsers;

/// <summary>
/// Response model for ListUsers operation
/// </summary>
public class ListUsersResult
{
   /// <summary>
   /// All users registered
   /// </summary>
   public List<User> Items { get; set; }
   
   public int TotalItems { get; set; }
   public int CurrentPage { get; set; }
   public int TotalPages { get; set; }
}
