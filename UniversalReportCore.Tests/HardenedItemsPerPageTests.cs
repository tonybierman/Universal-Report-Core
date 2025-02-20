using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using UniversalReportCore.HardQuerystringVariables.Hardened;

namespace UniversalReportCore.Tests
{
    public class HardenedItemsPerPageTests
    {
        [Fact]
        public void CheckSanity_NullValue_ShouldBeSane()
        {
            var hardenedIpp = new HardenedItemsPerPage(null);
            Assert.True(hardenedIpp.CheckSanity());
        }

        [Fact]
        public void CheckSanity_ValidValueWithinRange_ShouldBeSane()
        {
            var hardenedIpp = new HardenedItemsPerPage(500);
            Assert.True(hardenedIpp.CheckSanity());
        }

        [Fact]
        public void CheckSanity_MinBoundaryValue_ShouldBeSane()
        {
            var hardenedIpp = new HardenedItemsPerPage(0);
            Assert.True(hardenedIpp.CheckSanity());
        }

        [Fact]
        public void CheckSanity_MaxBoundaryValue_ShouldBeSane()
        {
            var hardenedIpp = new HardenedItemsPerPage(10000);
            Assert.True(hardenedIpp.CheckSanity());
        }

        [Fact]
        public void CheckSanity_ValueBelowMin_ShouldBeInsane()
        {
            var hardenedIpp = new HardenedItemsPerPage(-1);
            Assert.False(hardenedIpp.CheckSanity());
        }

        [Fact]
        public void CheckSanity_ValueAboveMax_ShouldBeInsane()
        {
            var hardenedIpp = new HardenedItemsPerPage(10001);
            Assert.False(hardenedIpp.CheckSanity());
        }

        [Fact]
        public void CheckSanity_ShouldSetIsValidEqualToIsSane()
        {
            var hardenedIpp = new HardenedItemsPerPage(100);
            hardenedIpp.CheckSanity();
            Assert.Equal(hardenedIpp.IsSane, hardenedIpp.IsValid);
        }
    }

}
