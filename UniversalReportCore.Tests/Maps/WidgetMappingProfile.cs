using AutoMapper;
using UniversalReportCoreTests.Data;
using UniversalReportCoreTests.ViewModels;

namespace UniversalReportCoreTests.Maps
{
    public class WidgetMappingProfile : Profile
    {
        public WidgetMappingProfile()
        {
            CreateMap<Widget, WidgetViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
