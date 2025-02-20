using AutoMapper;
using UniversalReportDemo.Data;

namespace ProductionPlanner.Maps
{
    public class CityPopulationMappingProfile : Profile
    {
        public CityPopulationMappingProfile()
        {
            CreateMap<CityPopulation, CityPopulationViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
