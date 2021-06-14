using System;

namespace MetricsAgent.Requests
{
    public class NetworkMetricGetByTimePeriodRequest
    {
        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
    }
}
