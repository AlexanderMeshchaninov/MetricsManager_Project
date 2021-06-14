using System;

namespace MetricsAgent.Requests
{
    public class CpuMetricGetByTimePeriodRequest
    {
        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
    }
}
