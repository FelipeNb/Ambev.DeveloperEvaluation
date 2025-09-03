using System.Globalization;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.ORM;
using Bogus;
using Microsoft.AspNetCore.Identity;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public static class DataSeeder
{
    
    private static readonly IPasswordHasher<string> Hasher = new PasswordHasher<string>();

    public static void Seed(DefaultContext context)
    {
        
        if (context.Users.Any() || context.Products.Any() || context.Carts.Any())
            return;

        var random = new Random();
        
        var userFaker = new Faker<User>()
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
            .RuleFor(u => u.Password, f => Hasher.HashPassword(null!, "Senha123!")) 
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.City, f => f.Address.City())
            .RuleFor(u => u.Street, f => f.Address.StreetName())
            .RuleFor(u => u.Number, f => int.Parse(f.Address.BuildingNumber()))
            .RuleFor(u => u.Zipcode, f => f.Address.ZipCode())
            .RuleFor(u => u.Latitude, f => f.Address.Latitude().ToString(CultureInfo.InvariantCulture))
            .RuleFor(u => u.Longitude, f => f.Address.Longitude().ToString(CultureInfo.InvariantCulture))
            .RuleFor(u => u.Role, f => f.PickRandom(UserRole.Customer, UserRole.Admin))
            .RuleFor(u => u.Status, f => f.PickRandom(UserStatus.Active, UserStatus.Suspended))
            .RuleFor(u => u.CreatedAt, f => DateTime.UtcNow);

        var users = userFaker.Generate(10);
        context.Users.AddRange(users);

        var productFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.Title, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()))
            .RuleFor(p => p.Category, f => f.Commerce.Categories(1).First())
            .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
            .RuleFor(p => p.RatingRate, f => f.Random.Decimal(1, 5))
            .RuleFor(p => p.RatingCount, f => f.Random.Int(1, 1000))
            .RuleFor(p => p.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(p => p.UpdatedAt, f => DateTime.UtcNow);

        var products = productFaker.Generate(20);
        context.Products.AddRange(products);

        context.SaveChanges();

        var cartFaker = new Faker<Cart>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(c => c.UserId, f => f.PickRandom(users).Id)
            .RuleFor(c => c.Branch, f => f.PickRandom("Online", "AllInOne LTDA", "Just do it"))
            .RuleFor(c => c.Cancelled, f => false)
            .RuleFor(c => c.Items, f =>
            {
                var cartItems = new List<CartProduct>();
                int numberOfItems = f.Random.Int(1, 5);

                for (int i = 0; i < numberOfItems; i++)
                {
                    var product = f.PickRandom(products);
                    cartItems.Add(new CartProduct
                    {
                        ProductId = product.Id,
                        Quantity = f.Random.Int(1, 20),
                        UnitPrice = product.Price
                    });
                }

                return cartItems;
            });

        var carts = cartFaker.Generate(15);
        carts.ForEach(s =>
        {
            s.Items.ForEach(f => f.CartId = s.Id);
            s.Items = s.Items
                .GroupBy(i => new { i.CartId, i.ProductId })
                .Select(g => g.First())
                .ToList();
        });
        context.Carts.AddRange(carts);

        context.SaveChanges();
    }

}