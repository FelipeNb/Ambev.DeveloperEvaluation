using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

/// <summary>
/// Response model for ListCarts operation
/// </summary>
public class ListCartsResult
{
   /// <summary>
   /// All carts registered
   /// </summary>
   public List<Cart> Items { get; set; }
   
   public int TotalItems { get; set; }
   public int CurrentPage { get; set; }
   public int TotalPages { get; set; }
}
