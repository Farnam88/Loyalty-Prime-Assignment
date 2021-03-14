using System;
using LoyaltyPrime.Models.Bases.Enums;
using LoyaltyPrime.Shared.Utilities.Extensions;
using Xunit;

namespace LoyaltyPrime.Infrastructure.Tests
{
    public class ExtensionsHelper
    {
        [Theory]
        [InlineData("input Object", true)]
        [InlineData("", true)]
        [InlineData("", false)]
        public void PreconditionCheckNull_ShouldThrowArgumentNullException_WhenObjectIsNull(string name,
            bool nullObject)
        {
            //Arrange
            Object obj = new object();

            //Act
            if (nullObject)
                Assert.Throws<ArgumentNullException>(() => Preconditions.CheckNull((object) null, name));
            if (!nullObject)
                Preconditions.CheckNull(obj);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void HasString_ShouldReturnFalse_WhenStringDoesNotHaveValue(string str)
        {
            //Act

            var result = StringExtensions.HasString(str);

            //Assert

            Assert.False(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ContainsString_ShouldReturnFalse_WhenStringDoesNotHaveValue(string str)
        {
            //Act

            var result = str.ContainsString();

            //Assert

            Assert.False(result);
        }

        [Theory]
        [InlineData("Farnam")]
        [InlineData(" Farnam ")]
        public void HasString_ShouldReturnTrue_WhenStringDoesNotHaveValue(string str)
        {
            //Act

            var result = StringExtensions.HasString(str);

            //Assert

            Assert.True(result);
        }

        [Theory]
        [InlineData("Farnam","FARNAM")]
        [InlineData(" Farnam ","FARNAM")]
        [InlineData(" Farnam jamshIDian ","FARNAM JAMSHIDIAN")]
        public void ToNormalize_ShouldTrimAndUpperCaseString_OnSuccess(string str,string expectedResult)
        {
            //Act

            var result = str.ToNormalize();

            //Assert

            Assert.Equal(expectedResult,result);
        }
        
        [Theory]
        [InlineData("Farnam")]
        [InlineData(" Farnam ")]
        public void ContainsString_ShouldReturnTrue_WhenStringDoesNotHaveValue(string str)
        {
            //Act

            var result = str.ContainsString();

            //Assert

            Assert.True(result);
        }

        [Theory]
        [InlineData("Active")]
        [InlineData("AcTivE")]
        [InlineData("active")]
        public void StringToEnum_ShouldReturnEnum_whenStringIsConvertibleToEnum(string enumStr)
        {
            //Act
            var enumResult = enumStr.StringToEnum<AccountStatus>();

            //Assert
            Assert.Equal(AccountStatus.Active, enumResult);
        }

        [Theory]
        [InlineData("Farnam")]
        [InlineData("FaRnam")]
        [InlineData("farnam")]
        public void StringToEnum_ShouldReturnDefaultEnumValue_whenStringIsNotConvertibleToEnum(string enumStr)
        {
            //Act
            var enumResult = enumStr.StringToEnum<AccountStatus>();

            //Assert
            Assert.Equal(default, enumResult);
        }
    }
}