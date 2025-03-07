using System.Collections.Generic;
using System.Linq;
using Moq;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.HardQuerystringVariables.Hardened;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class ReportQueryParamsTests
    {
        [Fact]
        public void IsHard_ShouldReturnFalse_WhenAnyVariableIsNotHard()
        {
            var reportQueryParams = CreateReportQueryParams(isHard: false, isSane: true, checkSanity: true);
            Assert.False(reportQueryParams.IsHard);
        }

        [Fact]
        public void IsSane_ShouldReturnFalse_WhenAnyVariableIsNotSane()
        {
            var reportQueryParams = CreateReportQueryParams(isHard: true, isSane: false, checkSanity: true);
            Assert.False(reportQueryParams.IsSane);
        }

        [Fact]
        public void CheckSanity_ShouldReturnTrue_WhenAllVariablesPassSanityCheck()
        {
            var reportQueryParams = CreateReportQueryParams(isHard: true, isSane: true, checkSanity: true);
            Assert.True(reportQueryParams.CheckSanity());
        }

        [Fact]
        public void CheckSanity_ShouldReturnFalse_WhenAnyVariableFailsSanityCheck()
        {
            var reportQueryParams = CreateReportQueryParams(isHard: true, isSane: true, checkSanity: false);
            Assert.False(reportQueryParams.CheckSanity());
        }

        // --- Helper Methods ---

        private ReportQueryParamsBase CreateReportQueryParams(bool isHard, bool isSane, bool checkSanity)
        {
            return new ReportQueryParamsBase(
                MockVariable<HardenedPagingIndex>(isHard, isSane, checkSanity),
                MockVariable<HardenedItemsPerPage>(isHard, isSane, checkSanity),
                MockVariable<HardenedColumnSort>(isHard, isSane, checkSanity),
                MockVariable<HardenedCohortIdentifiers>(isHard, isSane, checkSanity),
                MockVariable<HardenedReportSlug>(isHard, isSane, checkSanity),
                MockVariable<HardenedFilterKeys>(isHard, isSane, checkSanity)
            );
        }

        private T MockVariable<T>(bool isHard, bool isSane, bool checkSanity) where T : class, IHardVariable
        {
            var mock = new Mock<T>();
            mock.Setup(x => x.IsHard).Returns(isHard);
            mock.Setup(x => x.IsSane).Returns(isSane);
            mock.Setup(x => x.CheckSanity()).Returns(checkSanity);
            return mock.Object;
        }
    }
}
