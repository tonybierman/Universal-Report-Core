using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UniversalReportDemo.Data;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class CityPopulationTests : IClassFixture<TestStartupFixture>
    {
        private readonly TestStartupFixture _fixture;

        public CityPopulationTests(TestStartupFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Test_GetCityPops()
        {
            // Arrange
            var dbContext = _fixture.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Act
            var cityPops = await dbContext.CityPopulations.ToListAsync();

            // Assert
            Assert.NotEmpty(cityPops);
            Assert.Equal(36961, cityPops.Count);
        }
    }
}
