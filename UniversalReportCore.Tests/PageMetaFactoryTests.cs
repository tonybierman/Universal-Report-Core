using System;
using System.Collections.Generic;
using System.Reflection;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.ViewModels;
using Xunit;
using Moq;

namespace UniversalReportCore.Tests
{
    public class PageMetaFactoryTests
    {
        private readonly Mock<IPageMetaProvider> _mockProvider;
        private readonly PageMetaFactory _factory;

        public PageMetaFactoryTests()
        {
            _mockProvider = new Mock<IPageMetaProvider>();
            _mockProvider.Setup(p => p.Slug).Returns("TestPage");
            _mockProvider.Setup(p => p.GetPageMeta()).Returns(new PageMetaViewModel { Title = "Test", Subtitle = "Test Subtitle" });
            _mockProvider.Setup(p => p.ChartMeta).Returns((Dictionary<string, ChartMetaViewModel>?)null);
            _mockProvider.Setup(p => p.GetActionWellPartial()).Returns("TestPartial");

            var providers = new List<IPageMetaProvider> { _mockProvider.Object };
            _factory = new PageMetaFactory(providers);
        }

        [Fact]
        public void Constructor_ShouldInitializeProviders()
        {
            var providers = _factory.Providers;

            Assert.NotNull(providers);
            Assert.Single(providers);
        }

        [Fact]
        public void GetPageMeta_ShouldReturnCorrectMeta_ForValidSlug()
        {
            var result = _factory.GetPageMeta("TestPage");

            Assert.NotNull(result);
            Assert.Equal("Test", result.Title);
            Assert.Equal("Test Subtitle", result.Subtitle);
        }

        [Fact]
        public void GetPageMeta_WithPolicy_ShouldReturnMetaAndPolicy()
        {
            var providerWithPolicy = new TestPageMetaProviderWithPolicy("PolicyPage", new PageMetaViewModel { Title = "Policy Test" }, "TestPolicy");
            var providers = new List<IPageMetaProvider> { providerWithPolicy };
            var factory = new PageMetaFactory(providers);

            var result = factory.GetPageMeta("PolicyPage", out var policy);

            Assert.NotNull(result);
            Assert.Equal("Policy Test", result.Title);
        }

        [Fact]
        public void GetPageMeta_ShouldThrowInvalidOperationException_ForUnknownSlug()
        {
            var result = Record.Exception(() => _factory.GetPageMeta("NonExistentPage"));

            Assert.IsType<InvalidOperationException>(result);
            Assert.Equal("Unsupported meta for page: NonExistentPage", result.Message);
        }

        [Fact]
        public void GetChartMeta_ShouldReturnNull_ForProviderWithoutChartMeta()
        {
            var result = _factory.GetChartMeta("TestPage");

            Assert.Null(result);
        }

        [Fact]
        public void GetChartMeta_ShouldThrowInvalidOperationException_ForUnknownSlug()
        {
            var result = Record.Exception(() => _factory.GetChartMeta("NonExistentPage"));

            Assert.IsType<InvalidOperationException>(result);
            Assert.Equal("Unsupported meta for page: NonExistentPage", result.Message);
        }

        [Fact]
        public void GetActionWellPartial_ShouldReturnPartial_ForValidSlug()
        {
            var result = _factory.GetActionWellPartial("TestPage");

            Assert.Equal("TestPartial", result);
        }

        [Fact]
        public void GetActionWellPartial_ShouldThrowInvalidOperationException_ForUnknownSlug()
        {
            var result = Record.Exception(() => _factory.GetActionWellPartial("NonExistentPage"));

            Assert.IsType<InvalidOperationException>(result);
            Assert.Equal("Unsupported meta for page: NonExistentPage", result.Message);
        }
    }

    [PageMetaPolicy("TestPolicy")]
    public class TestPageMetaProviderWithPolicy : IPageMetaProvider
    {
        private readonly string _slug;
        private readonly PageMetaViewModel _meta;

        public TestPageMetaProviderWithPolicy(string slug, PageMetaViewModel meta, string policy)
        {
            _slug = slug;
            _meta = meta;
        }

        public string Slug => _slug;

        public string RouteLiteral => throw new NotImplementedException();

        public string CategorySlug => throw new NotImplementedException();

        public string TaxonomySlug => throw new NotImplementedException();

        public string? Description => throw new NotImplementedException();

        public bool IsPublished => throw new NotImplementedException();

        public Dictionary<string, ChartMetaViewModel>? ChartMeta => throw new NotImplementedException();

        public PageMetaViewModel GetPageMeta() => _meta;
        public ChartMetaViewModel? GetChartMeta() => null;
        public string? GetActionWellPartial() => null;
    }

    public class PageMetaPolicyAttribute : Attribute
    {
        public string Policy { get; }
        public PageMetaPolicyAttribute(string policy) => Policy = policy;
    }
}