using System;

namespace MetricsManager.Models
{
    public class RamMetrics
    {
        public int AgentId { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
