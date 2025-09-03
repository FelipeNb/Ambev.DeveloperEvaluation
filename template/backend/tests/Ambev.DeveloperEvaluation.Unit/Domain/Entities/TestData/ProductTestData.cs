using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class ProductTestData
{
    private static readonly Faker<Product> ProductFaker = new Faker<Product>()
        .RuleFor(p => p.Id, f => Guid.NewGuid())
        .RuleFor(p => p.Title, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription()[..50])
        .RuleFor(p => p.Price, f => f.Random.Decimal(1, 200))
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1).First())
        .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
        .RuleFor(p => p.RatingRate, f => f.Random.Decimal(1, 5))
        .RuleFor(p => p.RatingCount, f => f.Random.Int(1, 1000))
        .RuleFor(p => p.CreatedAt, f => DateTime.UtcNow)
        .RuleFor(p => p.UpdatedAt, f => DateTime.UtcNow);

    public static Product GenerateValidProduct()
    {
        return ProductFaker.Generate();
    }

}
