using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using UniversalReportCore.Helpers;

namespace UniversalReportCore.Tests
{
    public class SortHelperTests
    {
        private readonly List<IReportColumnDefinition> _columns;

        public SortHelperTests()
        {
            var provider = new CityPopulationDemoReportColumnProvider();
            _columns = provider.GetColumns();
        }

        [Fact]
        public void BaseSortKey_RemovesAscAndDesc()
        {
            Assert.Equal("City", SortHelper.BaseSortKey("CityDesc"));
            Assert.Equal("City", SortHelper.BaseSortKey("CityAsc"));
            Assert.Equal("City", SortHelper.BaseSortKey("City"));
            Assert.Null(SortHelper.BaseSortKey(null));
        }

        [Fact]
        public void IsDescending_ReturnsTrueIfEndsWithDesc()
        {
            Assert.True(SortHelper.IsDescending("CityDesc"));
            Assert.False(SortHelper.IsDescending("CityAsc"));
            Assert.False(SortHelper.IsDescending("City"));
            Assert.False(SortHelper.IsDescending(null));
        }

        [Fact]
        public void Description_ReturnsCorrectDescription()
        {
            Assert.Equal("by City Descending", SortHelper.Description("CityDesc"));
            Assert.Equal("by City Ascending", SortHelper.Description("CityAsc"));
            Assert.Equal("by City Ascending", SortHelper.Description("City")); // Default case
            Assert.Null(SortHelper.Description(null));
        }

        [Fact]
        public void ConfigureSort_SetsDescendingCorrectly()
        {
            SortHelper.ConfigureSort(_columns, "CityDesc");

            var cityColumn = _columns.Find(c => c.PropertyName == "City");
            var countryColumn = _columns.Find(c => c.PropertyName == "CountryOrArea");

            Assert.NotNull(cityColumn);
            Assert.True(cityColumn.IsSortDescending);

            Assert.NotNull(countryColumn);
            Assert.False(countryColumn.IsSortDescending);
        }
    }
}