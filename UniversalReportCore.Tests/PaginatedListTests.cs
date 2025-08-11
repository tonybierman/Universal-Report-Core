using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniversalReportCore;
using UniversalReportCoreTests.Data;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class PaginatedListTests : IDisposable
    {
        private readonly AcmeDbContext _dbContext;

        public PaginatedListTests()
        {
            var options = new DbContextOptionsBuilder<AcmeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AcmeDbContext(options);
            _dbContext.Database.EnsureCreated();
            SeedWidgetDatabase();
        }

        private void SeedWidgetDatabase()
        {
            _dbContext.Widgets.AddRange(new List<Widget>
            {
                new Widget { Id = 1, City = "New York", Value = 8419600, Year = 2020 },
                new Widget { Id = 2, City = "Los Angeles", Value = 3980400, Year = 2020 },
                new Widget { Id = 3, City = "Chicago", Value = 2716000, Year = 2020 },
                new Widget { Id = 4, City = "Houston", Value = 2328000, Year = 2020 },
                new Widget { Id = 5, City = "Phoenix", Value = 1690000, Year = 2020 }
            });
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnPaginatedList()
        {
            var query = _dbContext.Widgets.AsQueryable();
            var result = await PaginatedList<Widget>.CreateAsync(query, pageIndex: 1, pageSize: 2);
            result.EnsureTotalItemsCount(query.Count());

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(3, result.TotalPages);
        }

        [Fact]
        public async Task CreateAsync_ShouldCalculateTotalPagesCorrectly()
        {
            var query = _dbContext.Widgets.AsQueryable();
            var result = await PaginatedList<Widget>.CreateAsync(query, pageIndex: 1, pageSize: 2);
            result.EnsureTotalItemsCount(query.Count());

            Assert.Equal(3, result.TotalPages);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnEmptyList_WhenNoRecords()
        {
            _dbContext.Widgets.RemoveRange(_dbContext.Widgets);
            await _dbContext.SaveChangesAsync();
            var query = _dbContext.Widgets.AsQueryable();
            var result = await PaginatedList<Widget>.CreateAsync(query, pageIndex: 1, pageSize: 2);

            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.Equal(0, result.TotalPages);
            Assert.False(result.Any());
        }

        [Fact]
        public async Task CreateAsync_ShouldIndicateNextPageExists()
        {
            var query = _dbContext.Widgets.AsQueryable();
            var result = await PaginatedList<Widget>.CreateAsync(query, pageIndex: 1, pageSize: 2);
            result.EnsureTotalItemsCount(query.Count());

            Assert.True(result.HasNextPage);
            Assert.False(result.HasPreviousPage);
        }

        [Fact]
        public async Task CreateAsync_ShouldIndicatePreviousPageExists()
        {
            var query = _dbContext.Widgets.AsQueryable();
            var result = await PaginatedList<Widget>.CreateAsync(query, pageIndex: 2, pageSize: 2);
            result.EnsureTotalItemsCount(query.Count());

            Assert.True(result.HasPreviousPage);
            Assert.True(result.HasNextPage);
        }

        [Fact]
        public async Task CreateAsync_ShouldHandlePageSizeZero()
        {
            var query = _dbContext.Widgets.AsQueryable();
            var result = await PaginatedList<Widget>.CreateAsync(query, pageIndex: 1, pageSize: 0);
            result.EnsureTotalItemsCount(query.Count());

            Assert.Equal(5, result.Count);
            Assert.Equal(1, result.TotalPages);
            Assert.False(result.HasNextPage);
            Assert.False(result.HasPreviousPage);
        }

        [Fact]
        public async Task CreateMappedAsync_ShouldMapEntitiesCorrectly()
        {
            var query = _dbContext.Widgets.AsQueryable();
            var result = await PaginatedList<Widget>.CreateMappedAsync(
                query, pageIndex: 1, pageSize: 2,
                mapFunction: entity => new { entity.City, entity.Value });
            result.EnsureTotalItemsCount(query.Count());

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, x => x.City == "New York");
            Assert.Equal(3, result.TotalPages);
        }

        [Fact]
        public async Task CreateWithAggregatesAsync_ShouldIncludeAggregatesAndMeta()
        {
            var query = _dbContext.Widgets.AsQueryable();
            var aggregateFunc = new Func<IQueryable<Widget>, Task<Dictionary<string, dynamic>>>(
                q => Task.FromResult(new Dictionary<string, dynamic> { { "TotalValue", q.Sum(w => w.Value) } }));
            var metaFunc = new Func<IQueryable<Widget>, Task<Dictionary<string, dynamic>>>(
                q => Task.FromResult(new Dictionary<string, dynamic> { { "MaxYear", q.Max(w => w.Year) } }));

            var result = await PaginatedList<Widget>.CreateWithAggregatesAsync(
                query, pageIndex: 1, pageSize: 2, aggregateFunc, metaFunc);
            result.EnsureTotalItemsCount(query.Count());

            Assert.NotNull(result.Aggregates);
            Assert.NotNull(result.Meta);
            Assert.Equal(5, result.TotalItems);
            Assert.Equal(19134000, result.Aggregates["TotalValue"]);
            Assert.Equal(2020, result.Meta["MaxYear"]);
        }

        [Fact]
        public async Task CreateMappedWithAggregatesAsync_ShouldMapAndIncludeAggregates()
        {
            var query = _dbContext.Widgets.AsQueryable();
            var aggregateFunc = new Func<IQueryable<Widget>, Task<Dictionary<string, dynamic>>>(
                q => Task.FromResult(new Dictionary<string, dynamic> { { "TotalValue", q.Sum(w => w.Value) } }));
            var metaFunc = new Func<IQueryable<Widget>, Task<Dictionary<string, dynamic>>>(
                q => Task.FromResult(new Dictionary<string, dynamic> { { "MaxYear", q.Max(w => w.Year) } }));

            var result = await PaginatedList<Widget>.CreateMappedWithAggregatesAsync(
                query, pageIndex: 1, pageSize: 2,
                mapFunction: entity => new { entity.City, entity.Value },
                aggregateFunc, metaFunc);
            result.EnsureTotalItemsCount(query.Count());

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, x => x.City == "New York");
            Assert.NotNull(result.Aggregates);
            Assert.Equal(19134000, result.Aggregates["TotalValue"]);
            Assert.NotNull(result.Meta);
            Assert.Equal(2020, result.Meta["MaxYear"]);
            Assert.Equal(3, result.TotalPages);
        }

        [Fact]
        public async Task PaginatedList_ShouldSetPropertiesCorrectly()
        {
            var query = _dbContext.Widgets.AsQueryable();
            var result = await PaginatedList<Widget>.CreateAsync(query, pageIndex: 2, pageSize: 2);
            result.EnsureTotalItemsCount(query.Count());

            Assert.Equal(2, result.PageIndex);
            Assert.Equal(2, result.PageSize);
            Assert.Equal(5, result.TotalItems);
            Assert.Equal(3, result.TotalPages);
            Assert.Equal(3, result.StartItem);
            Assert.Equal(4, result.EndItem);
            Assert.Equal("Showing items 3 through 4 of 5", result.DisplayMessage);
            Assert.True(result.HasMultiplePages);
            Assert.True(result.Any());
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}