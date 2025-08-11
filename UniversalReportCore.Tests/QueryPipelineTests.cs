using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalReportCore.PagedQueries;
using Xunit;
using Moq;

namespace UniversalReportCore.Tests
{
    public class QueryPipelineTests
    {
        private readonly Mock<IQueryable<PipelineTestEntity>> _mockQuery;
        private readonly Mock<Func<IQueryable<PipelineTestEntity>, IQueryable<PipelineTestEntity>>> _mockUserFilter;
        private readonly Mock<Func<IQueryable<PipelineTestEntity>, IReportColumnDefinition[], Task<Dictionary<string, dynamic>>>> _mockComputeAggregates;
        private readonly Mock<Func<IQueryable<PipelineTestEntity>, int, IQueryable<PipelineTestEntity>>> _mockCohortPredicate;
        private readonly Mock<Func<IQueryable<PipelineTestEntity>, int[]?, IQueryable<PipelineTestEntity>>> _mockAggregatePredicate;
        private readonly Mock<Func<IQueryable<PipelineTestEntity>, Task<Dictionary<string, dynamic>>>> _mockEnsureMeta;
        private readonly IReportColumnDefinition[] _columns;

        public QueryPipelineTests()
        {
            _mockQuery = new Mock<IQueryable<PipelineTestEntity>>();
            _mockUserFilter = new Mock<Func<IQueryable<PipelineTestEntity>, IQueryable<PipelineTestEntity>>>();
            _mockComputeAggregates = new Mock<Func<IQueryable<PipelineTestEntity>, IReportColumnDefinition[], Task<Dictionary<string, dynamic>>>>();
            _mockCohortPredicate = new Mock<Func<IQueryable<PipelineTestEntity>, int, IQueryable<PipelineTestEntity>>>();
            _mockAggregatePredicate = new Mock<Func<IQueryable<PipelineTestEntity>, int[]?, IQueryable<PipelineTestEntity>>>();
            _mockEnsureMeta = new Mock<Func<IQueryable<PipelineTestEntity>, Task<Dictionary<string, dynamic>>>>();
            _columns = Array.Empty<IReportColumnDefinition>();
        }

        [Fact]
        public async Task BuildPagedQuery_NoCohortIds_ExecutesPipeline()
        {
            // Arrange
            _mockUserFilter.Setup(f => f(It.IsAny<IQueryable<PipelineTestEntity>>())).Returns(_mockQuery.Object);
            _mockComputeAggregates.Setup(f => f(It.IsAny<IQueryable<PipelineTestEntity>>(), _columns))
                .ReturnsAsync(new Dictionary<string, dynamic>());
            _mockEnsureMeta.Setup(f => f(It.IsAny<IQueryable<PipelineTestEntity>>()))
                .ReturnsAsync(new Dictionary<string, dynamic>());

            var pipeline = new QueryPipeline<PipelineTestEntity>()
                .AddStage(new UserFilterStage<PipelineTestEntity>(_mockUserFilter.Object))
                .AddStage(new CohortAggregateStage<PipelineTestEntity>(_columns, null, null, _mockComputeAggregates.Object, _mockCohortPredicate.Object, _mockAggregatePredicate.Object))
                .AddStage(new MetadataStage<PipelineTestEntity>(_mockEnsureMeta.Object));

            // Act
            var result = await pipeline.ExecuteAsync(_mockQuery.Object, _columns, 1, "name", 10, null, null);

            // Assert
            Assert.NotNull(result);
            //Assert.Equal(_mockQuery.Object, (await result.Query(null)));
            //Assert.NotNull(await result.Aggregates(null));
            //Assert.NotNull(await result.Metadata(null));
            _mockUserFilter.Verify(f => f(It.IsAny<IQueryable<PipelineTestEntity>>()), Times.Once());
            _mockComputeAggregates.Verify(f => f(It.IsAny<IQueryable<PipelineTestEntity>>(), _columns), Times.Once());
            _mockEnsureMeta.Verify(f => f(It.IsAny<IQueryable<PipelineTestEntity>>()), Times.Once());
        }

        [Fact]
        public void BuildPagedQuery_NullQuery_ThrowsException()
        {
            // Arrange
            var builder = new TestQueryBuilder();
            var columns = Array.Empty<IReportColumnDefinition>();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => builder.BuildPagedQuery(columns, 1, "name", 10, null, null, null));
        }
    }

    public class PipelineTestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestQueryBuilder
    {
        public virtual PagedQueryParameters<PipelineTestEntity> BuildPagedQuery(
            IReportColumnDefinition[] columns,
            int? pageIndex,
            string? sort,
            int? ipp,
            int[]? cohortIds,
            FilterConfig<PipelineTestEntity>? filterConfig = null,
            IQueryable<PipelineTestEntity>? reportQuery = null)
        {
            var pipeline = new QueryPipeline<PipelineTestEntity>()
                .AddStage(new UserFilterStage<PipelineTestEntity>(q => q))
                .AddStage(new CohortAggregateStage<PipelineTestEntity>(columns, cohortIds, filterConfig,
                    (q, c) => Task.FromResult(new Dictionary<string, dynamic>()),
                    (q, i) => q,
                    (q, i) => q))
                .AddStage(new MetadataStage<PipelineTestEntity>(q => Task.FromResult(new Dictionary<string, dynamic>())));
            var query = reportQuery ?? throw new InvalidOperationException("No query provided");
            return pipeline.ExecuteAsync(query, columns, pageIndex, sort, ipp, cohortIds, filterConfig).Result;
        }
    }
}