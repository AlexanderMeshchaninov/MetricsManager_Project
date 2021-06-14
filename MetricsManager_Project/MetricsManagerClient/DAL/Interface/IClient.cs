using MetricsManagerClient.Requests;
using MetricsManagerClient.Responses;

namespace MetricsManagerClient.DAL.Interface
{
    public interface IClient
    {
        CpuMetricsApiResponse GetAllCpuMetrics(CpuMetricsApiRequest request);
        HddMetricsApiResponse GetAllHddMetrics(HddMetricsApiRequest request);
        RamMetricsApiResponse GetAllRamMetrics(RamMetricsApiRequest request);
    }
}