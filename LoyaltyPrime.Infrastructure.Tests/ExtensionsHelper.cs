using System;
using LoyaltyPrime.DataAccessLayer.Shared.Utilities.Extensions;
using Xunit;

namespace LoyaltyPrime.Infrastructure.Tests
{
    public class ExtensionsHelper
    {
        [Fact]
        public void PreconditionCheckNull_ShouldThrowArgumentNullException_WhenObjectIsNull()
        {
            var obj = (object) null;
            Assert.Throws<ArgumentNullException>(() => Preconditions.CheckNull(obj));
        }

        [Fact]
        public void PreconditionCheckNull_ShouldThrowArgumentNullExceptionWithMessage_WhenObjectIsNull()
        {
            var obj = (object) null;
            Assert.Throws<ArgumentNullException>(() => Preconditions.CheckNull(obj, "entry object"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void HasString_ShouldReturnFalse_WhenStringDoesNotHaveValue(string str)
        {
            Assert.False(StringExtensions.HasString(str));
        }
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ContainsString_ShouldReturnFalse_WhenStringDoesNotHaveValue(string str)
        {
            Assert.False(str.ContainsString());
        }
        [Theory]
        [InlineData("Farnam")]
        [InlineData(" Farnam ")]
        public void HasString_ShouldReturnTrue_WhenStringDoesNotHaveValue(string str)
        {
            Assert.True(StringExtensions.HasString(str));
        }
        [Theory]
        [InlineData("Farnam")]
        [InlineData(" Farnam ")]
        public void ContainsString_ShouldReturnTrue_WhenStringDoesNotHaveValue(string str)
        {
            Assert.True(str.ContainsString());
        }
    }
}