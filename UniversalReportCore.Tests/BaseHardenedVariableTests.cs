using System;
using UniversalReportCore.HardQuerystringVariables;
using Xunit;
using FluentAssertions;

namespace UniversalReportCore.Tests
{
    public class BaseHardenedVariableTests
    {
        private class TestHardenedVariable : BaseHardenedVariable
        {
            public void SetValidity(bool valid) => IsValid = valid;
        }

        [Fact]
        public void Constructor_ShouldInitializeWithFalseValues()
        {
            // Arrange & Act
            var variable = new TestHardenedVariable();

            // Assert
            variable.IsSane.Should().BeFalse();
            variable.IsValid.Should().BeFalse();
            variable.IsHard.Should().BeFalse();
        }

        [Fact]
        public void CheckSanity_ShouldSetIsSaneToTrue()
        {
            // Arrange
            var variable = new TestHardenedVariable();

            // Act
            var result = variable.CheckSanity();

            // Assert
            result.Should().BeTrue();
            variable.IsSane.Should().BeTrue();
        }

        [Fact]
        public void IsHard_ShouldBeFalse_WhenOnlyIsSaneIsTrue()
        {
            // Arrange
            var variable = new TestHardenedVariable();

            // Act
            variable.CheckSanity();

            // Assert
            variable.IsHard.Should().BeFalse();
        }

        [Fact]
        public void IsHard_ShouldBeFalse_WhenOnlyIsValidIsTrue()
        {
            // Arrange
            var variable = new TestHardenedVariable();
            variable.SetValidity(true);

            // Assert
            variable.IsHard.Should().BeFalse();
        }

        [Fact]
        public void IsHard_ShouldBeTrue_WhenBothIsSaneAndIsValidAreTrue()
        {
            // Arrange
            var variable = new TestHardenedVariable();

            // Act
            variable.CheckSanity();
            variable.SetValidity(true);

            // Assert
            variable.IsHard.Should().BeTrue();
        }
    }
}
