using System;

namespace MetricsAgent.Requests
{
    public class HddMetricGetByTimePeriodRequest
    {
        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
    }
}
