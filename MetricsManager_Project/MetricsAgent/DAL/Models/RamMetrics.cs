using System;

namespace MetricsAgent.Models
{
    public class RamMetrics
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
