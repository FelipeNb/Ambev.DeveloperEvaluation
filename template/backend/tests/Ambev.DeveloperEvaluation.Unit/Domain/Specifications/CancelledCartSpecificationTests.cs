using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Specifications
{
    public class CancelledCartSpecificationTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsSatisfiedBy_ShouldValidateCartStatus(bool expectedResult)
        {
            // Arrange
            var cart = CartTestData.GenerateValidCart();

            // Act
            if(expectedResult)
                cart.Cancel();

            // Assert
            cart.Cancelled.Should().Be(expectedResult);
        }
    }
}