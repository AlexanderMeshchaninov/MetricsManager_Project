using System;

namespace MetricsManager.Models
{
    public class HddMetrics
    {
        public int AgentId { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
