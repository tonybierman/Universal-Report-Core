using System;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.ViewModels;
using UniversalReportDemo.Reports.CityPop;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class BasePageMetaProviderTests
    {
        private readonly CityPopulationDemoPageMetaProvider _provider;

        public BasePageMetaProviderTests()
        {
            // Arrange: Create an instance of the provider to test.
            _provider = new CityPopulationDemoPageMetaProvider();
        }

        [Fact]
        public void Provider_Should_Have_Correct_Slug()
        {
            // Act
            var slug = _provider.Slug;

            // Assert
            Assert.Equal("CityPopulationDemo", slug);
        }

        [Fact]
        public void Provider_Should_Have_Correct_CategorySlug()
        {
            // Act
            var categorySlug = _provider.CategorySlug;

            // Assert
            Assert.Equal("CityPopulationReports", categorySlug);
        }

        [Fact]
        public void GetPageMeta_Should_Return_Valid_Meta()
        {
            // Act
            var meta = _provider.GetPageMeta();

            // Assert
            Assert.NotNull(meta);
            Assert.Equal("Demo", meta.Title);
            Assert.Equal("Most Recent City Populations", meta.Subtitle);
        }
    }
}
