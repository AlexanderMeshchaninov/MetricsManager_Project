using System;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
    public class CpuMetricsApiResponse
    {
        public List<CpuMetricDto> Metrics { get; set; }
    }

    public class CpuMetricDto
    {
        public int AgentId { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}