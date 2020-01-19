using System;
using mat.coding.challenge.Attribute;
using mat.coding.challenge.Extensions;
using Xunit;

namespace mat.coding.challenge.test.Extensions
{
    [Topic("Test123")]
    public class TypeExtensionTests
    {
        [Fact]
        public void GetTopicName_Returns_Attribute_Name()
        {
            // Arrange

            // Act
            var sut = typeof(TypeExtensionTests).GetTopicName();

            // Assert
            Assert.Equal("Test123", sut);
        }

        [Fact]
        public void GetTopicName_Returns_emptyString()
        {
            // Arrange

            // Act
            var sut = typeof(Exception).GetTopicName();

            // Assert
            Assert.Equal(string.Empty, sut);
        }
    }
}
