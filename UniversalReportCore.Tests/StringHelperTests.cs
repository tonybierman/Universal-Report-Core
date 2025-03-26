using System;
using UniversalReportCore.Helpers;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class StringHelperTests
    {
        [Fact]
        public void SplitPascalCase_Should_Split_Simple_PascalCase_Correctly()
        {
            // Arrange
            string input = "HelloWorld";
            string expected = "Hello World";

            // Act
            string result = StringHelper.SplitPascalCase(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SplitPascalCase_Should_Return_Empty_String_For_Empty_Input()
        {
            // Arrange
            string input = "";
            string expected = "";

            // Act
            string result = StringHelper.SplitPascalCase(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SplitPascalCase_Should_Return_Null_For_Null_Input()
        {
            // Arrange
            string? input = null;
            string? expected = null;

            // Act
            string? result = StringHelper.SplitPascalCase(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SplitPascalCase_Should_Split_Multiple_Uppercase_Letters_Correctly()
        {
            // Arrange
            string input = "XMLParser";
            string expected = "XML Parser";

            // Act
            string result = StringHelper.SplitPascalCase(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SplitPascalCase_Should_Handle_Single_Character()
        {
            // Arrange
            string input = "A";
            string expected = "A";

            // Act
            string result = StringHelper.SplitPascalCase(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SplitPascalCase_Should_Handle_Lowercase_Input()
        {
            // Arrange
            string input = "lowercase";
            string expected = "lowercase";

            // Act
            string result = StringHelper.SplitPascalCase(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SplitPascalCase_Should_Split_Mixed_Case_Correctly()
        {
            // Arrange
            string input = "GetUserID";
            string expected = "Get User ID";

            // Act
            string result = StringHelper.SplitPascalCase(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SplitPascalCase_Should_Handle_Consecutive_Uppercase_At_End()
        {
            // Arrange
            string input = "MyAPI";
            string expected = "My API";

            // Act
            string result = StringHelper.SplitPascalCase(input);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}