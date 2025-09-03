using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using MediatR;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit;

public class CartTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    [Fact(DisplayName = "Validation should fail if cart has no items")]
    public void Given_EmptyCart_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        cart.Items.Clear(); // Empty the cart

        // Act
        var result = cart.Validate();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    
        result.Errors.Should().ContainSingle(e => e.Type == "Items" && e.Error == "NotEmptyValidator");
        result.Errors.Should().ContainSingle(e => e.Type == "TotalAmount" && e.Error == "GreaterThanValidator");
    }

    [Fact(DisplayName = "Cart total should sum all item totals correctly")]
    public void Given_ValidCart_When_CalculatingTotalAmount_Then_ShouldBeCorrect()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();

        // Act
        var calculatedTotal = cart.TotalAmount;
        var sumOfItemTotals = cart.Items.Sum(i => i.Total);

        // Assert
        calculatedTotal.Should().Be(sumOfItemTotals);
    }

    [Fact(DisplayName = "CartProduct total and discount percent should follow rules")]
    public void Given_ValidCartProducts_When_CalculatingTotals_Then_ShouldMatchDiscountRules()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();

        // Act & Assert
        foreach (var item in cart.Items)
        {
            var expectedDiscountPercent = item.Quantity switch
            {
                >= 10 => 0.20m,
                >= 4 => 0.10m,
                _ => 0.00m
            };

            var effectiveQuantity = Math.Min(item.Quantity, 20);
            var expectedTotal = decimal.Round(item.UnitPrice * effectiveQuantity * (1 - expectedDiscountPercent), 2);

            item.DiscountPercent.Should().Be(expectedDiscountPercent);
            item.Total.Should().Be(expectedTotal);
        }
    }

    [Fact(DisplayName = "Adding or updating items should work correctly")]
    public void Given_Cart_When_AddOrUpdateItem_Then_ItemShouldBeAddedOrUpdated()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        var product = ProductTestData.GenerateValidProduct();

        // Act - Add new item
        cart.AddOrUpdateItem(product.Id, 5, product.Price);
        var addedItem = cart.Items.FirstOrDefault(i => i.ProductId == product.Id);

        // Assert - Added correctly
        addedItem.Should().NotBeNull();
        addedItem.Quantity.Should().Be(5);
        addedItem.UnitPrice.Should().Be(product.Price);

        // Act - Update existing item
        cart.AddOrUpdateItem(product.Id, 10, product.Price + 5);

        // Assert - Updated correctly
        var updatedItem = cart.Items.First(i => i.ProductId == product.Id);
        updatedItem.Quantity.Should().Be(10);
        updatedItem.UnitPrice.Should().Be(product.Price + 5);
    }

    [Fact(DisplayName = "Removing an item should work correctly")]
    public void Given_Cart_When_RemoveItem_Then_ItemShouldBeRemoved()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        var itemToRemove = cart.Items.First();

        // Act
        cart.RemoveItem(itemToRemove.ProductId);

        // Assert
        cart.Items.Should().NotContain(i => i.ProductId == itemToRemove.ProductId);
    }

    [Fact(DisplayName = "Cancelling a cart should mark it as cancelled")]
    public void Given_Cart_When_Cancelled_Then_StatusShouldBeCancelled()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();

        // Act
        cart.Cancel();

        // Assert
        cart.Cancelled.Should().BeTrue();
    }
    
    [Theory(DisplayName = "Validation should get the corret discount")]
    [InlineData(20, 1, 0.20)]
    [InlineData(3, 1,0.0)]
    [InlineData(1, 1,0.0)]
    [InlineData(13, 1,0.20)]
    [InlineData(4, 1,0.10)]
    [InlineData(9, 1,0.10)]
    public void Given_Cart_When_Validated_Then_ShouldReturnValid(int quantity, decimal unitPrice, decimal discount)
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        cart.Items = new List<CartProduct>()
        {
            new CartProduct()
            {
                Quantity = quantity,
                UnitPrice = unitPrice
            }
        };
        var total = (quantity * unitPrice);
        // Assert
        Assert.True(cart.TotalAmount == total - (total * discount));
        cart.Items.ForEach(s =>
        {
            Assert.True(s.DiscountPercent == discount);
        });
        
    }

}