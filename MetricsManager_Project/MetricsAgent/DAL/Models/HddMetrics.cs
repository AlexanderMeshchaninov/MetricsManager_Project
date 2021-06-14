using System;

namespace MetricsAgent.Models
{
    public class HddMetrics
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
