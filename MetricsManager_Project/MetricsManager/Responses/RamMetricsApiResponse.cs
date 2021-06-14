using System;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
    public class RamMetricsApiResponse
    {
        public List<RamMetricDto> Metrics { get; set; }
    }
    public class RamMetricDto
    {
        public int AgentId { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}