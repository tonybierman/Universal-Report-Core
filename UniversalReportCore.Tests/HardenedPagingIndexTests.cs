using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using UniversalReportCore.HardQuerystringVariables.Hardened;

namespace UniversalReportCore.Tests
{


    public class HardenedPagingIndexTests
    {
        [Fact]
        public void CheckSanity_NullValue_ShouldBeSane()
        {
            var hardenedIndex = new HardenedPagingIndex(null);
            Assert.True(hardenedIndex.CheckSanity());
        }

        [Fact]
        public void CheckSanity_ValidValueWithinRange_ShouldBeSane()
        {
            var hardenedIndex = new HardenedPagingIndex(500);
            Assert.True(hardenedIndex.CheckSanity());
        }

        [Fact]
        public void CheckSanity_MinBoundaryValue_ShouldBeSane()
        {
            var hardenedIndex = new HardenedPagingIndex(1);
            Assert.True(hardenedIndex.CheckSanity());
        }

        [Fact]
        public void CheckSanity_MaxBoundaryValue_ShouldBeSane()
        {
            var hardenedIndex = new HardenedPagingIndex(10000);
            Assert.True(hardenedIndex.CheckSanity());
        }

        [Fact]
        public void CheckSanity_ValueBelowMin_ShouldBeInsane()
        {
            var hardenedIndex = new HardenedPagingIndex(0);
            Assert.False(hardenedIndex.CheckSanity());
        }

        [Fact]
        public void CheckSanity_ValueAboveMax_ShouldBeInsane()
        {
            var hardenedIndex = new HardenedPagingIndex(10001);
            Assert.False(hardenedIndex.CheckSanity());
        }

        [Fact]
        public void Validate_NullValue_ShouldBeValid()
        {
            var hardenedIndex = new HardenedPagingIndex(null);
            Assert.True(hardenedIndex.Validate(50));
        }

        [Fact]
        public void Validate_ValueWithinTotalPages_ShouldBeValid()
        {
            var hardenedIndex = new HardenedPagingIndex(10);
            Assert.True(hardenedIndex.Validate(20));
        }

        [Fact]
        public void Validate_ValueExceedingTotalPages_ShouldBeInvalid()
        {
            var hardenedIndex = new HardenedPagingIndex(30);
            Assert.False(hardenedIndex.Validate(20));
        }
    }

}
