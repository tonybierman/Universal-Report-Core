using System;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.Tests.Reports.Acme;
using UniversalReportCore.ViewModels;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class BasePageMetaProviderTests
    {
        private readonly AcmeTestPageMetaProvider _provider;

        public BasePageMetaProviderTests()
        {
            // Arrange: Create an instance of the provider to test.
            _provider = new AcmeTestPageMetaProvider();
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

        [Fact]
        public void Provider_Should_Be_Published_By_Default()
        {
            // Act
            var isPublished = _provider.IsPublished;

            // Assert
            Assert.True(isPublished);
        }

        [Fact]
        public void Provider_Should_Have_Null_RouteLiteral_By_Default()
        {
            // Act
            var routeLiteral = _provider.RouteLiteral;

            // Assert
            Assert.Equal("/Reports/Index", routeLiteral);
        }

        [Fact]
        public void Provider_Should_Have_Null_TaxonomySlug_By_Default()
        {
            // Act
            var taxonomySlug = _provider.TaxonomySlug;

            // Assert
            Assert.Equal("CityPopulationReports", taxonomySlug);
        }

        [Fact]
        public void Provider_Should_Have_Null_Description_By_Default()
        {
            // Act
            var description = _provider.Description;

            // Assert
            Assert.Null(description);
        }
    }
}