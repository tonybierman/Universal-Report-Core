using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Threading.Tasks;
using UniversalReportCore;
using UniversalReportCore.PagedQueries;
using UniversalReportCore.Tests;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UniversalReportCore.Tests
{
    public class CohortTestEntity
    {
        public int Id { get; set; }
        public int CohortId { get; set; }
        public decimal Value { get; set; }
        public DateOnly PunchDate { get; set; }
    }

    public class CohortAggregateStageTests
    {
        private readonly Mock<IReportColumnDefinition> _columnMock;
        private readonly Mock<IFilterProvider<CohortTestEntity>> _filterProviderMock;
        private readonly Mock<IFilterFactory<CohortTestEntity>> _filterFactoryMock;
        private readonly List<CohortTestEntity> _data;
        private readonly IQueryable<CohortTestEntity> _query;

        public CohortAggregateStageTests()
        {
            _columnMock = new Mock<IReportColumnDefinition>();
            _columnMock.Setup(c => c.PropertyName).Returns("Value");
            _columnMock.Setup(c => c.Aggregation).Returns(AggregationType.Sum);

            _filterProviderMock = new Mock<IFilterProvider<CohortTestEntity>>();
            //_filterProviderMock.Setup(p => p.ValidateFilter(It.IsAny<string[]>())).Returns(true);

            _filterFactoryMock = new Mock<IFilterFactory<CohortTestEntity>>();

            _data = new List<CohortTestEntity>
            {
                new CohortTestEntity { Id = 1, CohortId = 1, Value = 10, PunchDate = DateOnly.FromDateTime(DateTime.Now) },
                new CohortTestEntity { Id = 2, CohortId = 1, Value = 20, PunchDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-2)) },
                new CohortTestEntity { Id = 3, CohortId = 2, Value = 30, PunchDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)) }
            };
            _query = _data.AsQueryable();
        }

        [Fact]
        public async Task ExecuteAsync_NoCohortIds_ComputesAggregates()
        {
            // Arrange
            var columns = new[] { _columnMock.Object };
            var computeAggregates = new Func<IQueryable<CohortTestEntity>, IReportColumnDefinition[], Task<Dictionary<string, dynamic>>>(
                (q, c) => Task.FromResult(new Dictionary<string, dynamic> { { "Sum_Value", q.Sum(x => x.Value) } }));
            var stage = new CohortAggregateStage<CohortTestEntity>(
                columns,
                null,
                null,
                computeAggregates,
                (q, id) => q,
                (q, ids) => q);
            var input = new PipelineResult<CohortTestEntity>(_query);

            // Act
            var result = await stage.ExecuteAsync(input);

            // Assert
            Assert.Equal(60m, result.Aggregates["Sum_Value"]);
            Assert.Equal(_query, result.Query);
        }

        [Fact]
        public async Task ExecuteAsync_WithCohortIds_ComputesCohortAggregates()
        {
            // Arrange
            var columns = new[] { _columnMock.Object };
            var cohortIds = new[] { 1, 2 };
            var computeAggregates = new Func<IQueryable<CohortTestEntity>, IReportColumnDefinition[], Task<Dictionary<string, dynamic>>>(
                (q, c) => Task.FromResult(new Dictionary<string, dynamic> { { "Sum_Value", q.Sum(x => x.Value) } }));
            var stage = new CohortAggregateStage<CohortTestEntity>(
                columns,
                cohortIds,
                null,
                computeAggregates,
                (q, id) => q.Where(x => x.CohortId == id),
                (q, ids) => q);
            var input = new PipelineResult<CohortTestEntity>(_query);

            // Act
            var result = await stage.ExecuteAsync(input);

            // Assert
            Assert.Equal(60m, result.Aggregates["Sum_Value"]);
            Assert.Equal(30m, result.Aggregates["Sum_Value_1"]);
            Assert.Equal(30m, result.Aggregates["Sum_Value_2"]);
            Assert.Equal(_query, result.Query);
        }

        //[Fact]
        //public async Task ExecuteAsync_WithFilterConfig_AppliesFilter()
        //{
        //    // Arrange
        //    var columns = new[] { _columnMock.Object };
        //    _filterFactoryMock.Setup(f => f.BuildPredicate(It.IsAny<IEnumerable<string>>()))
        //        .Returns((Expression<Func<CohortTestEntity, bool>>)(x => x.PunchDate >= DateOnly.FromDateTime(DateTime.Now.AddDays(-3)) && x.PunchDate <= DateOnly.FromDateTime(DateTime.Now)));
        //    var filterConfig = new FilterConfig<CohortTestEntity>(
        //        _filterProviderMock.Object,
        //        (FilterFactory<CohortTestEntity>)_filterFactoryMock.Object, // Use mocked IFilterFactory
        //        new[] { "Days_3" });

        //    var computeAggregates = new Func<IQueryable<CohortTestEntity>, IReportColumnDefinition[], Task<Dictionary<string, dynamic>>>(
        //        (q, c) => Task.FromResult(new Dictionary<string, dynamic> { { "Sum_Value", q.Sum(x => x.Value) } }));
        //    var stage = new CohortAggregateStage<CohortTestEntity>(
        //        columns,
        //        null,
        //        filterConfig,
        //        computeAggregates,
        //        (q, id) => q,
        //        (q, ids) => q);
        //    var input = new PipelineResult<CohortTestEntity>(_query);

        //    // Act
        //    var result = await stage.ExecuteAsync(input);

        //    // Assert
        //    Assert.Equal(30m, result.Aggregates["Sum_Value"]);
        //    Assert.Equal(_query.Where(x => x.PunchDate >= DateOnly.FromDateTime(DateTime.Now.AddDays(-3)) && x.PunchDate <= DateOnly.FromDateTime(DateTime.Now)).ToList(), result.Query.ToList());
        //}
    }
}