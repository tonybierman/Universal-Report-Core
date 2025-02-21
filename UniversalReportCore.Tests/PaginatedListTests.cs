using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniversalReportCore;
using UniversalReportDemo.Data;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class PaginatedListTests : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;

        public PaginatedListTests()
        {
            // ** Set up In-Memory Database **
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureCreated();

            // ** Seed test data **
            SeedCityPopulationDatabase();
        }

        /// <summary>
        /// Seeds the in-memory database with test data.
        /// </summary>
        private void SeedCityPopulationDatabase()
        {
            _dbContext.CityPopulations.AddRange(new List<CityPopulation>
            {
                new CityPopulation { Id = 1, City = "New York", Value = 8419600, Year = 2020 },
                new CityPopulation { Id = 2, City = "Los Angeles", Value = 3980400, Year = 2020 },
                new CityPopulation { Id = 3, City = "Chicago", Value = 2716000, Year = 2020 },
                new CityPopulation { Id = 4, City = "Houston", Value = 2328000, Year = 2020 },
                new CityPopulation { Id = 5, City = "Phoenix", Value = 1690000, Year = 2020 }
            });

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tests whether `PaginatedList<T>` correctly paginates data.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ShouldReturnPaginatedList()
        {
            // Arrange
            var query = _dbContext.CityPopulations.AsQueryable();

            // Act
            var result = await PaginatedList<CityPopulation>.CreateAsync(query, pageIndex: 1, pageSize: 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count); // Page size is 2
            Assert.Equal(3, result.TotalPages); // Total items = 5, so 3 pages of 2 items each
        }

        /// <summary>
        /// Tests whether `PaginatedList<T>` correctly calculates page count.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ShouldCalculateTotalPagesCorrectly()
        {
            // Arrange
            var query = _dbContext.CityPopulations.AsQueryable();

            // Act
            var result = await PaginatedList<CityPopulation>.CreateAsync(query, pageIndex: 1, pageSize: 2);

            // Assert
            Assert.Equal(3, result.TotalPages); // 5 items with page size of 2 should give 3 total pages
        }

        /// <summary>
        /// Tests whether `PaginatedList<T>` correctly handles an empty dataset.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ShouldReturnEmptyList_WhenNoRecords()
        {
            // Arrange
            _dbContext.CityPopulations.RemoveRange(_dbContext.CityPopulations);
            await _dbContext.SaveChangesAsync();

            var query = _dbContext.CityPopulations.AsQueryable();

            // Act
            var result = await PaginatedList<CityPopulation>.CreateAsync(query, pageIndex: 1, pageSize: 2);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.Equal(0, result.TotalPages);
        }

        /// <summary>
        /// Tests whether `PaginatedList<T>` correctly identifies if there is a next page.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ShouldIndicateNextPageExists()
        {
            // Arrange
            var query = _dbContext.CityPopulations.AsQueryable();

            // Act
            var result = await PaginatedList<CityPopulation>.CreateAsync(query, pageIndex: 1, pageSize: 2);

            // Assert
            Assert.True(result.HasNextPage);
        }

        /// <summary>
        /// Tests whether `PaginatedList<T>` correctly identifies if there is a previous page.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ShouldIndicatePreviousPageExists()
        {
            // Arrange
            var query = _dbContext.CityPopulations.AsQueryable();

            // Act
            var result = await PaginatedList<CityPopulation>.CreateAsync(query, pageIndex: 2, pageSize: 2);

            // Assert
            Assert.True(result.HasPreviousPage);
        }

        /// <summary>
        /// Tests `CreateMappedAsync` for converting entities into a different type.
        /// </summary>
        [Fact]
        public async Task CreateMappedAsync_ShouldMapEntitiesCorrectly()
        {
            // Arrange
            var query = _dbContext.CityPopulations.AsQueryable();

            // Act
            var result = await PaginatedList<CityPopulation>.CreateMappedAsync(
                query, pageIndex: 1, pageSize: 2,
                mapFunction: entity => new { entity.City, entity.Value });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count); // Page size is 2
            Assert.Contains(result, x => x.City == "New York");
        }

        /// <summary>
        /// Cleans up the in-memory database after each test.
        /// </summary>
        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
