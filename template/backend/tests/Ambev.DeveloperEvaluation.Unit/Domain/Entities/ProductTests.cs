using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Xunit;

public class ProductTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    [Fact]
    public void ProductTestData_ShouldGenerateValidProduct()
    {
        // Arrange & Act
        var product = ProductTestData.GenerateValidProduct();

        // Assert
        product.Should().NotBeNull();
        product.Id.Should().NotBeEmpty();
        product.Title.Should().NotBeNullOrEmpty();
        product.Price.Should().BeGreaterThan(0);
        product.Category.Should().NotBeNullOrEmpty();
        product.RatingRate.Should().BeInRange(1, 5);
        product.RatingCount.Should().BeGreaterThan(0);
    }
    
    [Fact(DisplayName = "Validation should fail for invalid product data")]
    public void Given_InvalidProductData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var product = new Product
        {
            Title = "",               // Invalid: empty
            Price = -5m,              // Invalid: negative price
            Category = "",            // Invalid: empty
            Description = null,       // Optional, but let's include as test case
            Image = null,             // Optional
            RatingRate = 6m,          // Invalid: out of expected range 1-5
            RatingCount = -1           // Invalid: negative
        };

        // Act
        var result = product.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact(DisplayName = "Validation should pass for valid product data")]
    public void Given_ValidProductData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();

        // Act
        var result = product.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}

