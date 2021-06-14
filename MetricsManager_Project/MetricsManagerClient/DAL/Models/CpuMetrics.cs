using System;

namespace MetricsManagerClient.Models
{
    public class CpuMetrics
    {
        public int AgentId { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
