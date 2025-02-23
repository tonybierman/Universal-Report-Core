using AutoMapper;
using UniversalReportHeavyDemo.Data;
using UniversalReportHeavyDemo.ViewModels;

namespace ProductionPlanner.Maps
{
    public class NationalGdpMappingProfile : Profile
    {
        public NationalGdpMappingProfile()
        {
            CreateMap<NationalGdp, NationalGdpViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
