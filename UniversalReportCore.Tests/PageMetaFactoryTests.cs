using System.Collections.Generic;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.Tests.Reports.Acme;
using UniversalReportCore.ViewModels;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class PageMetaFactoryTests
    {
        private readonly List<IPageMetaProvider> _providers;
        private readonly PageMetaFactory _factory;

        public PageMetaFactoryTests()
        {
            // Use CityPopulationDemoPageMetaProvider as the concrete provider
            var cityPopProvider = new AcmeDemoPageMetaProvider();

            _providers = new List<IPageMetaProvider> { cityPopProvider };
            _factory = new PageMetaFactory(_providers);
        }

        [Fact]
        public void Constructor_ShouldInitializeProviders()
        {
            // Act
            var providers = _factory.Providers;

            // Assert
            Assert.NotNull(providers);
            Assert.Single(providers); // Ensure only one provider is initialized
        }

        [Fact]
        public void GetPageMeta_ShouldReturnCorrectMeta_ForValidSlug()
        {
            // Act
            var result = _factory.GetPageMeta("CityPopulationDemo");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Demo", result.Title);
            Assert.Equal("Most Recent City Populations", result.Subtitle);
        }

        [Fact]
        public void GetPageMeta_ShouldThrowInvalidOperationException_ForUnknownSlug()
        {
            // Act
            var result = Record.Exception(() => _factory.GetPageMeta("NonExistentReport"));

            // Assert
            Assert.IsType<InvalidOperationException>(result);
        }


        [Fact]
        public void GetChartMeta_ShouldReturnNull_ForProviderWithoutChartMeta()
        {
            // Act
            var result = _factory.GetChartMeta("CityPopulationDemo");

            // Assert
            Assert.Null(result); // Since CityPopulationDemoPageMetaProvider does not override GetChartMeta()
        }
    }
}
