using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Responses;

namespace MetricsAgent
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CpuMetrics, CpuMetricDto>();
            CreateMap<DotNetMetrics, DotNetMetricDto>();
            CreateMap<HddMetrics, HddMetricDto>();
            CreateMap<NetworkMetrics, NetworkMetricDto>();
            CreateMap<RamMetrics, RamMetricDto>();
        }
    }
}
