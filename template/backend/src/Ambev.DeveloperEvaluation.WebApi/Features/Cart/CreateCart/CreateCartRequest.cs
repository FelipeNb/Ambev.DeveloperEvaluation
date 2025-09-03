using Ambev.DeveloperEvaluation.WebApi.Features.Cart.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart
{
    public class CreateCartRequest
    {
        public Guid UserId { get; set; }
        public string Branch { get; set; } = string.Empty;
        public List<CartItem> Items { get; set; } = new();
    }

   
}
