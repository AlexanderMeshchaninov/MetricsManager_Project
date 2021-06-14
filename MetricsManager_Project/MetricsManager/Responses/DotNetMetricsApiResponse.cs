using System;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
    public class DotNetMetricsApiResponse
    {
        public List<DotNetMetricDto> Metrics { get; set; }
    }

    public class DotNetMetricDto
    {
        public int AgentId { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}