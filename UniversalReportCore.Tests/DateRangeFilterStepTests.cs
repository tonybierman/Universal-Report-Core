using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using UniversalReportCore.PagedQueries;
using UniversalReportCore.Services.QueryPipeline;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class DateRangeFilterStepTests
    {
        public class DateFilterTestEntity
        {
            public DateTime CreatedAt { get; set; }
            public DateFilterNestedTestEntity? Nested { get; set; }
        }

        public class DateFilterNestedTestEntity
        {
            public DateTime NestedDate { get; set; }
        }

        private readonly IQueryable<DateFilterTestEntity> _data;

        public DateRangeFilterStepTests()
        {
            _data = new[]
            {
                new DateFilterTestEntity { CreatedAt = new DateTime(2025, 1, 1), Nested = new DateFilterNestedTestEntity { NestedDate = new DateTime(2025, 1, 1) } },
                new DateFilterTestEntity { CreatedAt = new DateTime(2025, 2, 1), Nested = new DateFilterNestedTestEntity { NestedDate = new DateTime(2025, 2, 1) } },
                new DateFilterTestEntity { CreatedAt = new DateTime(2025, 3, 1), Nested = null }
            }.AsQueryable();
        }

        [Fact]
        public void Execute_WithSimplePropertyDateFilter_ReturnsFilteredQuery()
        {
            var step = new DateRangeFilterStep<DateFilterTestEntity>();
            var parameters = new PagedQueryParameters<DateFilterTestEntity>(
                columns: Array.Empty<IReportColumnDefinition>(),
                pageIndex: null,
                sort: null,
                itemsPerPage: null,
                cohortIds: null,
                filterConfig: null)
            {
                DateFilter = new DateRangeFilter(new DateTime(2025, 1, 15), new DateTime(2025, 2, 15), "CreatedAt")
            };

            var result = step.Execute(_data, parameters).ToList();
            Assert.Equal(1, result.Count);
            Assert.Equal(new DateTime(2025, 2, 1), result[0].CreatedAt);
        }

        //[Fact]
        //public void Execute_WithNestedPropertyDateFilter_ReturnsFilteredQuery()
        //{
        //    var step = new DateRangeFilterStep<TestEntity>();
        //    var parameters = new PagedQueryParameters<TestEntity>(
        //        columns: Array.Empty<IReportColumnDefinition>(),
        //        pageIndex: null,
        //        sort: null,
        //        itemsPerPage: null,
        //        cohortIds: null,
        //        filterConfig: null)
        //    {
        //        DateFilter = new DateRangeFilter(new DateTime(2025, 1, 15), new DateTime(2025, 2, 15), "Nested.NestedDate")
        //    };

        //    var result = step.Execute(_data, parameters).ToList();
        //    Assert.Equal(1, result.Count);
        //    Assert.Equal(new DateTime(2025, 2, 1), result[0].Nested!.NestedDate);
        //}

        [Fact]
        public void Execute_InvalidPropertyName_ThrowsException()
        {
            var step = new DateRangeFilterStep<DateFilterTestEntity>();
            var parameters = new PagedQueryParameters<DateFilterTestEntity>(
                columns: Array.Empty<IReportColumnDefinition>(),
                pageIndex: null,
                sort: null,
                itemsPerPage: null,
                cohortIds: null,
                filterConfig: null)
            {
                DateFilter = new DateRangeFilter(new DateTime(2025, 1, 1), new DateTime(2025, 2, 1), "InvalidProperty")
            };

            var ex = Assert.Throws<ArgumentException>(() => step.Execute(_data, parameters).ToList());
            Assert.Equal("Invalid property name.", ex.Message);
        }

        [Fact]
        public void Execute_OnlyStartDate_ReturnsFilteredQuery()
        {
            var step = new DateRangeFilterStep<DateFilterTestEntity>();
            var parameters = new PagedQueryParameters<DateFilterTestEntity>(
                columns: Array.Empty<IReportColumnDefinition>(),
                pageIndex: null,
                sort: null,
                itemsPerPage: null,
                cohortIds: null,
                filterConfig: null)
            {
                DateFilter = new DateRangeFilter(new DateTime(2025, 2, 1), DateTime.MinValue, "CreatedAt")
            };

            var result = step.Execute(_data, parameters).ToList();
            Assert.Equal(2, result.Count);
            Assert.Contains(result, e => e.CreatedAt == new DateTime(2025, 2, 1));
            Assert.Contains(result, e => e.CreatedAt == new DateTime(2025, 3, 1));
        }

        [Fact]
        public void Execute_OnlyEndDate_ReturnsFilteredQuery()
        {
            var step = new DateRangeFilterStep<DateFilterTestEntity>();
            var parameters = new PagedQueryParameters<DateFilterTestEntity>(
                columns: Array.Empty<IReportColumnDefinition>(),
                pageIndex: null,
                sort: null,
                itemsPerPage: null,
                cohortIds: null,
                filterConfig: null)
            {
                DateFilter = new DateRangeFilter(DateTime.MinValue, new DateTime(2025, 2, 1), "CreatedAt")
            };

            var result = step.Execute(_data, parameters).ToList();
            Assert.Equal(2, result.Count);
            Assert.Contains(result, e => e.CreatedAt == new DateTime(2025, 1, 1));
            Assert.Contains(result, e => e.CreatedAt == new DateTime(2025, 2, 1));
        }
    }
}