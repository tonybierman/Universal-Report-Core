using System;
using System.Linq;
using Xunit;
using UniversalReportCore.HardQuerystringVariables.Hardened;
using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCore.Tests
{
    public class HardenedCohortIdentifiersTests
    {
        [Fact]
        public void CheckSanity_NullValue_ShouldBeSane()
        {
            var hardenedCohorts = new HardenedCohortIdentifiers(null);
            Assert.True(hardenedCohorts.CheckSanity());
        }

        [Fact]
        public void CheckSanity_EmptyArray_ShouldBeSane()
        {
            var hardenedCohorts = new HardenedCohortIdentifiers(Array.Empty<int>());
            Assert.True(hardenedCohorts.CheckSanity());
        }

        [Fact]
        public void CheckSanity_ValidIds_ShouldBeSane()
        {
            var hardenedCohorts = new HardenedCohortIdentifiers(new int[] { 5, 10, 20 });
            Assert.True(hardenedCohorts.CheckSanity());
        }

        [Fact]
        public void CheckSanity_OutOfRangeIds_ShouldBeInsane()
        {
            var hardenedCohorts = new HardenedCohortIdentifiers(new int[] { 0, 10001 });
            Assert.False(hardenedCohorts.CheckSanity());
        }

        [Fact]
        public void CheckSanity_ExceedsMaxArraySize_ShouldBeInsane()
        {
            var largeArray = Enumerable.Range(1, 101).ToArray();
            var hardenedCohorts = new HardenedCohortIdentifiers(largeArray);
            Assert.False(hardenedCohorts.CheckSanity());
        }

        [Fact]
        public void CheckSanity_DuplicateIds_ShouldBeInsane()
        {
            var hardenedCohorts = new HardenedCohortIdentifiers(new int[] { 5, 10, 10, 20 });
            Assert.False(hardenedCohorts.CheckSanity());
        }

        [Fact]
        public void Validate_NullValue_ShouldBeValid()
        {
            var hardenedCohorts = new HardenedCohortIdentifiers(null);
            Assert.True(hardenedCohorts.Validate());
        }

        [Fact]
        public void Validate_EmptyArray_ShouldBeValid()
        {
            var hardenedCohorts = new HardenedCohortIdentifiers(Array.Empty<int>());
            Assert.True(hardenedCohorts.Validate());
        }

        [Fact]
        public void Validate_ValidIds_ShouldBeValid()
        {
            var cohorts = new[]
            {
            new Cohort { Id = 1 },
            new Cohort { Id = 2 },
            new Cohort { Id = 3 }
        };

            var hardenedCohorts = new HardenedCohortIdentifiers(new int[] { 1, 2 });
            Assert.True(hardenedCohorts.Validate(cohorts));
        }

        [Fact]
        public void Validate_InvalidIds_ShouldBeInvalid()
        {
            var cohorts = new[]
            {
            new Cohort { Id = 1 },
            new Cohort { Id = 2 }
        };

            var hardenedCohorts = new HardenedCohortIdentifiers(new int[] { 3, 4 });
            Assert.False(hardenedCohorts.Validate(cohorts));
        }

        [Fact]
        public void Validate_MixedValidAndInvalidIds_ShouldBeInvalid()
        {
            var cohorts = new[]
            {
            new Cohort { Id = 1 },
            new Cohort { Id = 2 }
        };

            var hardenedCohorts = new HardenedCohortIdentifiers(new int[] { 1, 3 });
            Assert.False(hardenedCohorts.Validate(cohorts));
        }
    }

}