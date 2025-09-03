using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class CartTestData
{
    private static readonly Faker<Cart> CartFaker = new Faker<Cart>()
        .RuleFor(c => c.Id, f => Guid.NewGuid())
        .RuleFor(c => c.UserId, f => Guid.NewGuid())
        .RuleFor(c => c.SaleNumber, f => f.Random.Int(1, 5))
        .RuleFor(c => c.Branch, f => f.PickRandom("Online", "AllInOne LTDA", "Just do it"))
        .RuleFor(c => c.Cancelled, f => false)
        .RuleFor(c => c.Items, f =>
        {
            var cartItems = new List<CartProduct>();
            int numberOfItems = f.Random.Int(1, 5);

            for (int i = 0; i < numberOfItems; i++)
            {
                var product = ProductTestData.GenerateValidProduct();
                cartItems.Add(new CartProduct
                {
                    ProductId = product.Id,
                    Quantity = f.Random.Int(1, 20),
                    UnitPrice = product.Price
                });
            }

            return cartItems;
        });

   

    public static Cart GenerateValidCart()
    {
        
        var cart =  CartFaker.Generate();
        cart.Items.ForEach(f => f.CartId = cart.Id);
        cart.Items = cart.Items
            .GroupBy(i => new { i.CartId, i.ProductId })
            .Select(g => g.First())
            .ToList();

        return cart;
    }

}