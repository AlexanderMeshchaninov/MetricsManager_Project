using AutoMapper;
using MetricsManager.Models;
using MetricsManager.Responses;

namespace MetricsManager
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
            CreateMap<MyNamespace.CpuMetricsResponse, CpuMetricsApiResponse>();
            CreateMap<MyNamespace.DotNetMetricResponse, DotNetMetricsApiResponse>();
            CreateMap<MyNamespace.HddMetricResponse, HddMetricsApiResponse>();
            CreateMap<MyNamespace.NetworkMetricResponse, NetworkMetricsApiResponse>();
            CreateMap<MyNamespace.RamMetricsResponse, RamMetricsApiResponse>();
            CreateMap<MyNamespace.CpuMetricDto, CpuMetricDto>();
            CreateMap<MyNamespace.DotNetMetricDto, DotNetMetricDto>();
            CreateMap<MyNamespace.HddMetricDto, HddMetricDto>();
            CreateMap<MyNamespace.NetworkMetricDto, NetworkMetricDto>();
            CreateMap<MyNamespace.RamMetricDto, RamMetricDto>();
        }
    }
}
