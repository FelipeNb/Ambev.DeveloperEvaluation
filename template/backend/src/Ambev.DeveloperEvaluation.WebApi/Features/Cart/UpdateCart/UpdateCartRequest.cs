using Ambev.DeveloperEvaluation.WebApi.Features.Cart.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.UpdateCart
{
    public class UpdateCartRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Branch { get; set; } = string.Empty;
        public List<CartItem> Items { get; set; } = new();
    }

   
}
