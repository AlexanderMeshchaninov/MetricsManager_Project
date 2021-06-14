using System;
using System.Collections.Generic;

namespace MetricsAgent.Responses
{
    public class RamMetricsResponse
    {
        public List<RamMetricDto> Metrics { get; set; }
    }
    public class RamMetricDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
