﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.HardQuerystringVariables.Hardened;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.Ui.ViewModels;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class FacetedBrowserViewModelTests
    {
        public class MockFilterProvider : IFilterProviderBase
        {
            private readonly IDictionary<string, string[]> _facetKeys;
            public MockFilterProvider(IDictionary<string, string[]> facetKeys) => _facetKeys = facetKeys;
            public IDictionary<string, string[]> GetFacetKeys() => _facetKeys;

            Dictionary<string, List<string>> IFilterProviderBase.GetFacetKeys()
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void FacetedBrowserViewModel_Default_Construction_Should_Have_Null_Properties()
        {
            // Arrange & Act
            var viewModel = new FacetedBrowserViewModel();

            // Assert
            Assert.Null(viewModel.Params);
            Assert.Null(viewModel.FilterOptions);
            Assert.Null(viewModel.PageIndex);
            Assert.Null(viewModel.SortOrder);
        }

        [Fact]
        public void FacetedBrowserViewModel_Params_Should_Accept_And_Return_Value()
        {
            // Arrange
            var viewModel = new FacetedBrowserViewModel();
            var queryParams = new ReportQueryParamsBase(
                pi: new HardenedPagingIndex(0),
                ipp: new HardenedItemsPerPage(10),
                sortOrder: new HardenedColumnSort("asc"),
                cohortIds: new HardenedCohortIdentifiers(new int[] { 1, 2, 3 }),
                slug: new HardenedReportSlug("report-slug"),
                filterKeys: new HardenedFilterKeys(new string[] { "key1", "key2" })
            );

            // Act
            viewModel.Params = queryParams;

            // Assert
            Assert.NotNull(viewModel.Params);
            Assert.Equal(0, viewModel.Params.Pi.Value);
            Assert.Equal(10, viewModel.Params.Ipp.Value);
            Assert.Equal("asc", viewModel.Params.SortOrder.Value);
            Assert.Equal(new int[] { 1, 2, 3 }, viewModel.Params.CohortIds.Value);
            Assert.Equal("report-slug", viewModel.Params.Slug.Value);
            Assert.Equal(new string[] { "key1", "key2" }, viewModel.Params.FilterKeys.Value);
            Assert.False(viewModel.Params.CheckSanity());
            Assert.False(viewModel.Params.IsSane);
            Assert.False(viewModel.Params.IsHard);
        }

        [Fact]
        public void FacetedBrowserViewModel_FilterOptions_Should_Accept_And_Return_List()
        {
            // Arrange
            var viewModel = new FacetedBrowserViewModel();
            var filterOptions = new List<(string Heading, List<SelectListItem> Options)>
            {
                ("Category", new List<SelectListItem>
                {
                    new SelectListItem { Text = "Option1", Value = "1" },
                    new SelectListItem { Text = "Option2", Value = "2" }
                })
            };

            // Act
            viewModel.FilterOptions = filterOptions;

            // Assert
            Assert.NotNull(viewModel.FilterOptions);
            Assert.Single(viewModel.FilterOptions);
            Assert.Equal("Category", viewModel.FilterOptions[0].Heading);
            Assert.Equal(2, viewModel.FilterOptions[0].Options.Count);
        }

        [Fact]
        public void FacetedBrowserViewModel_PageIndex_Should_Not_Affect_Params_Pi()
        {
            // Arrange
            var viewModel = new FacetedBrowserViewModel();
            var queryParams = new ReportQueryParamsBase(
                pi: new HardenedPagingIndex(2),
                ipp: new HardenedItemsPerPage(10),
                sortOrder: new HardenedColumnSort("asc"),
                cohortIds: new HardenedCohortIdentifiers(new int[] { 1, 2, 3 }),
                slug: new HardenedReportSlug("report-slug"),
                filterKeys: new HardenedFilterKeys(new string[] { "key1" })
            );
            viewModel.Params = queryParams;

            // Act
            viewModel.PageIndex = 3;

            // Assert
            Assert.Equal(3, viewModel.PageIndex);
            Assert.Equal(2, viewModel.Params.Pi.Value);
        }

        [Fact]
        public void FacetedBrowserViewModel_SortOrder_Should_Not_Affect_Params_SortOrder()
        {
            // Arrange
            var viewModel = new FacetedBrowserViewModel();
            var queryParams = new ReportQueryParamsBase(
                pi: new HardenedPagingIndex(0),
                ipp: new HardenedItemsPerPage(10),
                sortOrder: new HardenedColumnSort("asc"),
                cohortIds: new HardenedCohortIdentifiers(new int[] { 1, 2, 3 }),
                slug: new HardenedReportSlug("report-slug"),
                filterKeys: new HardenedFilterKeys(new string[] { "key1" })
            );
            viewModel.Params = queryParams;

            // Act
            viewModel.SortOrder = "desc";

            // Assert
            Assert.Equal("desc", viewModel.SortOrder);
            Assert.Equal("asc", viewModel.Params.SortOrder.Value);
        }

        [Fact]
        public void FacetedBrowserViewModel_Params_FilterKeys_Contains_Should_Work()
        {
            // Arrange
            var viewModel = new FacetedBrowserViewModel();
            var queryParams = new ReportQueryParamsBase(
                pi: new HardenedPagingIndex(0),
                ipp: new HardenedItemsPerPage(10),
                sortOrder: new HardenedColumnSort("asc"),
                cohortIds: new HardenedCohortIdentifiers(new int[] { 1, 2, 3 }),
                slug: new HardenedReportSlug("report-slug"),
                filterKeys: new HardenedFilterKeys(new string[] { "key1", "key2" })
            );
            viewModel.Params = queryParams;

            // Act & Assert
            Assert.True(viewModel.Params.FilterKeys.Contains("key1"));
            Assert.False(viewModel.Params.FilterKeys.Contains("key3"));
        }

        [Fact]
        public void FacetedBrowserViewModel_Params_FilterKeys_Validate_Should_Throw_NotImplementedException()
        {
            // Arrange
            var viewModel = new FacetedBrowserViewModel();
            var queryParams = new ReportQueryParamsBase(
                pi: new HardenedPagingIndex(0),
                ipp: new HardenedItemsPerPage(10),
                sortOrder: new HardenedColumnSort("asc"),
                cohortIds: new HardenedCohortIdentifiers(new int[] { 1, 2, 3 }),
                slug: new HardenedReportSlug("report-slug"),
                filterKeys: new HardenedFilterKeys(new string[] { "key1", "key3" })
            );
            viewModel.Params = queryParams;
            var filterProvider = new MockFilterProvider(new Dictionary<string, string[]>
            {
                { "facet1", new[] { "key1", "key2" } },
                { "facet2", new[] { "key4" } }
            });

            // Act & Assert
            var exception = Assert.Throws<NotImplementedException>(() =>
                viewModel.Params.FilterKeys.Validate(filterProvider));
        }

        [Fact]
        public void FacetedBrowserViewModel_Params_FilterKeys_Null_Should_Be_Valid()
        {
            // Arrange
            var viewModel = new FacetedBrowserViewModel();
            var queryParams = new ReportQueryParamsBase(
                pi: new HardenedPagingIndex(0),
                ipp: new HardenedItemsPerPage(10),
                sortOrder: new HardenedColumnSort("asc"),
                cohortIds: new HardenedCohortIdentifiers(new int[] { 1, 2, 3 }),
                slug: new HardenedReportSlug("report-slug"),
                filterKeys: new HardenedFilterKeys(null)
            );
            viewModel.Params = queryParams;
            var filterProvider = new MockFilterProvider(new Dictionary<string, string[]>());

            // Act
            bool isValid = viewModel.Params.FilterKeys.Validate(filterProvider);

            // Assert
            Assert.True(isValid);
            Assert.True(viewModel.Params.FilterKeys.IsValid);
        }
    }
}