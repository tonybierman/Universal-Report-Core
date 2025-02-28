using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class ReportPageHelperFactoryBaseTests
    {
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly TestReportPageHelperFactory _factory;

        public ReportPageHelperFactoryBaseTests()
        {
            _mockServiceProvider = new Mock<IServiceProvider>();
            _factory = new TestReportPageHelperFactory(_mockServiceProvider.Object);
        }

        [Fact]
        public void GetHelper_ReturnsHelper_WhenReportTypeExists()
        {
            // Arrange
            string reportType = "TestReport";
            var expectedHelper = new Mock<IReportPageHelperBase>().Object;

            _mockServiceProvider.Setup(sp => sp.GetService(typeof(IReportPageHelper<TestEntity, TestViewModel>)))
                                .Returns(expectedHelper);

            _factory.AddHelper(reportType, typeof(TestEntity), typeof(TestViewModel));

            // Act
            var result = _factory.GetHelper(reportType);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedHelper, result);
        }

        [Fact]
        public void GetHelper_ThrowsArgumentException_WhenReportTypeNotFound()
        {
            // Arrange
            string invalidReportType = "InvalidReport";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _factory.GetHelper(invalidReportType));
            Assert.Contains($"No PageHelper found for report type {invalidReportType}", ex.Message);
        }

        [Fact]
        public void GetHelper_ThrowsInvalidOperationException_WhenDIResolutionFails()
        {
            // Arrange
            string reportType = "TestReport";
            _factory.AddHelper(reportType, typeof(TestEntity), typeof(TestViewModel));

            _mockServiceProvider.Setup(sp => sp.GetService(It.IsAny<Type>())).Returns(null); // Simulating DI failure

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _factory.GetHelper(reportType));
            Assert.Contains("Failed to resolve", ex.Message);
        }

        // Helper class to expose AddHelper for testing
        private class TestReportPageHelperFactory : ReportPageHelperFactoryBase
        {
            public TestReportPageHelperFactory(IServiceProvider serviceProvider) : base(serviceProvider) { }

            public void AddHelper(string reportType, Type entityType, Type viewModelType)
            {
                _helperMap[reportType] = (entityType, viewModelType);
            }
        }

        // Dummy test classes
        private class TestEntity { }
        private class TestViewModel { }
    }
}
