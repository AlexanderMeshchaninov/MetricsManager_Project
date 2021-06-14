using MetricsManager.Requests;
using MetricsManager.Responses;

namespace MetricsManager.Client
{
    public interface IMetricsAgentClient
    {
        CpuMetricsApiResponse GetCpuMetrics(CpuMetricsApiRequest request);
        DotNetMetricsApiResponse GetDotNetMetrics(DotNetMetricsApiRequest request);
        HddMetricsApiResponse GetHddMetrics(HddMetricsApiRequest request);
        NetworkMetricsApiResponse GetNetworkMetrics(NetworkMetricsApiRequest request);
        RamMetricsApiResponse GetRamMetrics(RamMetricsApiRequest request);
    }
}