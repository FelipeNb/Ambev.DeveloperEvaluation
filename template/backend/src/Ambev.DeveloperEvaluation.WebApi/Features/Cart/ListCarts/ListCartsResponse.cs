using Ambev.DeveloperEvaluation.WebApi.Features.Cart.GetCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.ListCarts;

public class ListCartsResponse
{
   public int TotalItems { get; set; }
   public int CurrentPage { get; set; }
   public int TotalPages { get; set; }
   public List<GetCartResponse> Items { get; set; } = new();

}
