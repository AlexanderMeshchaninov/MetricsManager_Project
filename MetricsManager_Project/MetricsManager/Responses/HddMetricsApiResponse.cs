using System;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
    public class HddMetricsApiResponse
    {
        public List<HddMetricDto> Metrics { get; set; }
    }
    public class HddMetricDto
    {
        public int AgentId { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}