using System;

namespace MetricsAgent.Requests
{
    public class RamMetricGetByTimePeriodRequest
    {
        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
    }
}
