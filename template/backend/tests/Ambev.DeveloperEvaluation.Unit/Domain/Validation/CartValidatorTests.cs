using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the CartValidator class.
/// Tests cover validation of all cart properties including user, sale number,
/// branch, items, and business rules like maximum quantity per item.
/// </summary>
public class CartValidatorTests
{
    private readonly CartValidator _validator;

    public CartValidatorTests()
    {
        _validator = new CartValidator();
    }

    /// <summary>
    /// Tests that validation passes when all cart properties are valid.
    /// </summary>
    [Fact(DisplayName = "Valid cart should pass all validation rules")]
    public void Given_ValidCart_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();

        // Act
        var result = _validator.TestValidate(cart);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that validation fails when quantity exceeds maximum limit of 20.
    /// </summary>
    [Fact(DisplayName = "Cart with item quantity above 20 should fail validation")]
    public void Given_CartWithQuantityAbove20_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        cart.Items.First().Quantity = 21; // Exceeds maximum of 20

        // Act
        var result = _validator.TestValidate(cart);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].Quantity");
        result.Errors.Should().ContainSingle(e => 
            e.ErrorMessage == "Quantity must be less than or equal to 20");
    }

    /// <summary>
    /// Tests that validation passes when quantity is exactly at the maximum limit of 20.
    /// </summary>
    [Fact(DisplayName = "Cart with item quantity exactly 20 should pass validation")]
    public void Given_CartWithQuantityExactly20_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        cart.Items.First().Quantity = 20; // Exactly at maximum

        // Act
        var result = _validator.TestValidate(cart);

        // Assert
        result.ShouldNotHaveValidationErrorFor("Items[0].Quantity");
    }

    /// <summary>
    /// Tests that validation fails when cart has no items.
    /// </summary>
    [Fact(DisplayName = "Cart with no items should fail validation")]
    public void Given_CartWithNoItems_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        cart.Items.Clear();

        // Act
        var result = _validator.TestValidate(cart);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Items);
    }

    /// <summary>
    /// Tests that validation fails when cart has duplicate product IDs.
    /// </summary>
    [Fact(DisplayName = "Cart with duplicate product IDs should fail validation")]
    public void Given_CartWithDuplicateProductIds_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        var duplicateProductId = cart.Items.First().ProductId;
        cart.Items.Add(new CartProduct
        {
            Id = Guid.NewGuid(),
            CartId = cart.Id,
            ProductId = duplicateProductId,
            Quantity = 5,
            UnitPrice = 10.00m
        });

        // Act
        var result = _validator.TestValidate(cart);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Items);
        result.Errors.Should().ContainSingle(e => 
            e.ErrorMessage == "Duplicate ProductIds are not allowed");
    }

    /// <summary>
    /// Tests that validation fails when item quantity is zero.
    /// </summary>
    [Fact(DisplayName = "Cart with item quantity zero should fail validation")]
    public void Given_CartWithZeroQuantity_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        cart.Items.First().Quantity = 0;

        // Act
        var result = _validator.TestValidate(cart);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].Quantity");
        result.Errors.Should().ContainSingle(e => 
            e.ErrorMessage == "Quantity must be greater than zero");
    }

    /// <summary>
    /// Tests that validation fails when item quantity is negative.
    /// </summary>
    [Fact(DisplayName = "Cart with negative item quantity should fail validation")]
    public void Given_CartWithNegativeQuantity_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        cart.Items.First().Quantity = -1;

        // Act
        var result = _validator.TestValidate(cart);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].Quantity");
        result.Errors.Should().ContainSingle(e => 
            e.ErrorMessage == "Quantity must be greater than zero");
    }

    /// <summary>
    /// Tests that validation fails when item unit price is zero.
    /// </summary>
    [Fact(DisplayName = "Cart with item unit price zero should fail validation")]
    public void Given_CartWithZeroUnitPrice_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        cart.Items.First().UnitPrice = 0;

        // Act
        var result = _validator.TestValidate(cart);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].UnitPrice");
        result.Errors.Should().ContainSingle(e => 
            e.ErrorMessage == "Unit price must be greater than zero");
    }

    /// <summary>
    /// Tests that validation fails when item unit price is negative.
    /// </summary>
    [Fact(DisplayName = "Cart with negative item unit price should fail validation")]
    public void Given_CartWithNegativeUnitPrice_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        cart.Items.First().UnitPrice = -10.00m;

        // Act
        var result = _validator.TestValidate(cart);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].UnitPrice");
        result.Errors.Should().ContainSingle(e => 
            e.ErrorMessage == "Unit price must be greater than zero");
    }

    /// <summary>
    /// Tests that validation fails when cart has no branch specified.
    /// </summary>
    [Fact(DisplayName = "Cart with no branch should fail validation")]
    public void Given_CartWithNoBranch_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        cart.Branch = string.Empty;

        // Act
        var result = _validator.TestValidate(cart);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Branch);
        result.Errors.Should().ContainSingle(e => 
            e.ErrorMessage == "Branch must be specified");
    }

    /// <summary>
    /// Tests that validation fails when cart has no sale number.
    /// </summary>
    [Fact(DisplayName = "Cart with no sale number should fail validation")]
    public void Given_CartWithNoSaleNumber_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        cart.SaleNumber = 0;

        // Act
        var result = _validator.TestValidate(cart);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SaleNumber);
        result.Errors.Should().ContainSingle(e => 
            e.ErrorMessage == "Sale number must be specified");
    }
}