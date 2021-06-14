using System;

namespace MetricsAgent.Requests
{
    public class DotNetMetricGetByTimePeriodRequest
    {
        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
    }
}
