using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalReportCore;
using UniversalReportCore.PagedQueries;
using UniversalReportCore.Tests.Reports.Acme;
using UniversalReportCoreTests.Data;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class BasePagedQueryProviderTests : IDisposable
    {
        private readonly AcmeDbContext _dbContext;
        private readonly PagedAcmeQueryProvider _queryProvider;

        public BasePagedQueryProviderTests()
        {
            var options = new DbContextOptionsBuilder<AcmeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AcmeDbContext(options);
            _queryProvider = new PagedAcmeQueryProvider(_dbContext);

            SeedDatabase();
        }

        [Fact]
        public async Task ComputeAggregatesWithCohortsAsync_ReturnsAggregates_Sum()
        {
            var columns = new IReportColumnDefinition[]
            {
                new ReportColumnDefinition { PropertyName = "City" },
                new ReportColumnDefinition { PropertyName = "Value", Aggregation = AggregationType.Sum },
                new ReportColumnDefinition { PropertyName = "IntValue", Aggregation = AggregationType.Sum }
            };
            var query = _dbContext.Widgets.AsQueryable();
            var result = await _queryProvider.ComputeAggregates(query, columns);
            Assert.NotNull(result);
            Assert.DoesNotContain("City", result);
            Assert.Contains("Value", result);
            Assert.Contains("IntValue", result);
            Assert.Equal(3000000m, result["Value"]);
            Assert.Equal(3000000, result["IntValue"]);
        }

        [Fact]
        public async Task ComputeAggregatesWithCohortsAsync_ReturnsAggregates_Average()
        {
            var columns = new IReportColumnDefinition[]
            {
                new ReportColumnDefinition { PropertyName = "City" },
                new ReportColumnDefinition { PropertyName = "Value", Aggregation = AggregationType.Average },
                new ReportColumnDefinition { PropertyName = "IntValue", Aggregation = AggregationType.Average }
            };
            var query = _dbContext.Widgets.AsQueryable();
            var result = await _queryProvider.ComputeAggregates(query, columns);
            Assert.NotNull(result);
            Assert.DoesNotContain("City", result);
            Assert.Contains("Value", result);
            Assert.Contains("IntValue", result);
            Assert.Equal(1000000m, result["Value"]);
            Assert.Equal(1000000.0, result["IntValue"]);
        }

        [Fact]
        public async Task ComputeAggregatesWithCohortsAsync_ReturnsAggregates_Count()
        {
            var columns = new IReportColumnDefinition[]
            {
                new ReportColumnDefinition { PropertyName = "City" },
                new ReportColumnDefinition { PropertyName = "City", Aggregation = AggregationType.Count }
            };
            var query = _dbContext.Widgets.AsQueryable();
            var result = await _queryProvider.ComputeAggregates(query, columns);
            Assert.NotNull(result);
            Assert.Contains("City", result);
            Assert.Equal(0, result["City"]); // Non-collection property returns 0
        }

        [Fact]
        public async Task ComputeAggregatesWithCohortsAsync_ReturnsAggregates_Min()
        {
            var columns = new IReportColumnDefinition[]
            {
                new ReportColumnDefinition { PropertyName = "City" },
                new ReportColumnDefinition { PropertyName = "Value", Aggregation = AggregationType.Min },
                new ReportColumnDefinition { PropertyName = "IntValue", Aggregation = AggregationType.Min }
            };
            var query = _dbContext.Widgets.AsQueryable();
            var result = await _queryProvider.ComputeAggregates(query, columns);
            Assert.NotNull(result);
            Assert.DoesNotContain("City", result);
            Assert.Contains("Value", result);
            Assert.Contains("IntValue", result);
            Assert.Equal(800000m, result["Value"]);
            Assert.Equal(800000, result["IntValue"]);
        }

        [Fact]
        public async Task ComputeAggregatesWithCohortsAsync_ReturnsAggregates_Max()
        {
            var columns = new IReportColumnDefinition[]
            {
                new ReportColumnDefinition { PropertyName = "City" },
                new ReportColumnDefinition { PropertyName = "Value", Aggregation = AggregationType.Max },
                new ReportColumnDefinition { PropertyName = "IntValue", Aggregation = AggregationType.Max }
            };
            var query = _dbContext.Widgets.AsQueryable();
            var result = await _queryProvider.ComputeAggregates(query, columns);
            Assert.NotNull(result);
            Assert.DoesNotContain("City", result);
            Assert.Contains("Value", result);
            Assert.Contains("IntValue", result);
            Assert.Equal(1200000m, result["Value"]);
            Assert.Equal(1200000, result["IntValue"]);
        }

        [Fact]
        public async Task ComputeAggregatesWithCohortsAsync_ReturnsAggregates_StandardDeviation()
        {
            var columns = new IReportColumnDefinition[]
            {
                new ReportColumnDefinition { PropertyName = "Value", Aggregation = AggregationType.StandardDeviation }
            };
            var query = _dbContext.Widgets.AsQueryable();
            var result = await _queryProvider.ComputeAggregates(query, columns);
            Assert.NotNull(result);
            Assert.Contains("Value", result);
            // Geometric mean-based standard deviation: sqrt(Σ((ln(x) - ln(geometric mean))^2) / N)
            // Values: 800000, 1000000, 1200000
            // Geometric mean = (800000 * 1000000 * 1200000)^(1/3) ≈ 970146
            // Log differences: ln(800000/970146), ln(1000000/970146), ln(1200000/970146)
            // Variance ≈ (0.1927^2 + 0.0309^2 + 0.2134^2) / 3 ≈ 0.0278
            // StdDev ≈ sqrt(0.0278) * 970146 ≈ 161837
            Assert.InRange((decimal)result["Value"], 161000, 164000);
        }

        [Fact]
        public async Task ComputeAggregates_NullPropertyName_SkipsAggregation()
        {
            var columns = new IReportColumnDefinition[]
            {
                new ReportColumnDefinition { PropertyName = null, Aggregation = AggregationType.Sum }
            };
            var query = _dbContext.Widgets.AsQueryable();
            var result = await _queryProvider.ComputeAggregates(query, columns);
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ComputeAggregates_InvalidProperty_SkipsAggregation()
        {
            var columns = new IReportColumnDefinition[]
            {
                new ReportColumnDefinition { PropertyName = "InvalidProperty", Aggregation = AggregationType.Sum }
            };
            var query = _dbContext.Widgets.AsQueryable();
            var result = await _queryProvider.ComputeAggregates(query, columns);
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void EnsureAggregateQuery_ShouldReturn_UnmodifiedQuery_WhenNoCohorts()
        {
            var query = _dbContext.Widgets.AsQueryable();
            int[]? cohortIds = null;
            var resultQuery = _queryProvider.EnsureAggregatePredicate(query, cohortIds);
            Assert.Equal(query.Count(), resultQuery.Count());
        }

        [Fact]
        public void EnsureCohortQuery_ShouldReturn_FilteredQuery()
        {
            var query = _dbContext.Widgets.AsQueryable();
            int cohortId = 1;
            var resultQuery = _queryProvider.EnsureCohortPredicate(query, cohortId);
            Assert.NotNull(resultQuery);
        }

        [Fact]
        //public async Task BuildPagedQuery_NoQueryProvided_ThrowsInvalidOperationException()
        //{
        //    var columns = new IReportColumnDefinition[] { new ReportColumnDefinition { PropertyName = "City" } };
        //    await Assert.ThrowsAsync<InvalidOperationException>(() => _queryProvider.BuildPagedQuery(columns, null, null, null, null, null, null));
        //}

        private void SeedDatabase()
        {
            // Clear existing data to avoid tracking conflicts
            _dbContext.Widgets.RemoveRange(_dbContext.Widgets);
            _dbContext.Widgets.AddRange(new List<Widget>
            {
                new Widget { Id = 1, City = "New York", Value = 800000m, IntValue = 800000 },
                new Widget { Id = 2, City = "Los Angeles", Value = 1200000m, IntValue = 1200000 },
                new Widget { Id = 3, City = "Chicago", Value = 1000000m, IntValue = 1000000 }
            });
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }

    //public class ReportColumnDefinition : IReportColumnDefinition
    //{
    //    public string DisplayName { get; set; } = string.Empty;
    //    public bool IsSortable { get; set; }
    //    public bool IsDisplayKey { get; set; }
    //    public string? DefaultSort { get; set; }
    //    public string? CssClass { get; set; }
    //    public bool IsSortDescending { get; set; }
    //    public string PropertyName { get; set; } = string.Empty;
    //    public string ViewModelName { get; set; } = string.Empty;
    //    public string? RenderPartial { get; set; }
    //    public bool HideInPortrait { get; set; }
    //    public AggregationType Aggregation { get; set; }
    //    public Type? ViewModelType => null;
    //}
}