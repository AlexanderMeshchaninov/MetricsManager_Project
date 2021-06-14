using AutoMapper;
using MetricsManagerClient.Models;
using MetricsManagerClient.Responses;

namespace MetricsManagerClient
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CpuMetrics, CpuMetricDto>();
            CreateMap<HddMetrics, HddMetricDto>();
            CreateMap<RamMetrics, RamMetricDto>();
            CreateMap<MetricsManagerGeneratedClient.CpuMetricsApiResponse, CpuMetricsApiResponse>();
            CreateMap<MetricsManagerGeneratedClient.HddMetricsApiResponse, HddMetricsApiResponse>();
            CreateMap<MetricsManagerGeneratedClient.RamMetricsApiResponse, RamMetricsApiResponse>();
            CreateMap<MetricsManagerGeneratedClient.CpuMetricDto, CpuMetricDto>();
            CreateMap<MetricsManagerGeneratedClient.HddMetricDto, HddMetricDto>();
            CreateMap<MetricsManagerGeneratedClient.RamMetricDto, RamMetricDto>();
        }
    }
}
