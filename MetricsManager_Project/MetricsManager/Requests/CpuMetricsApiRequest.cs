using System;

namespace MetricsManager.Requests
{
    public class CpuMetricsApiRequest
    {
        public string ClientBaseAddress { get; set; }
        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
    }
}