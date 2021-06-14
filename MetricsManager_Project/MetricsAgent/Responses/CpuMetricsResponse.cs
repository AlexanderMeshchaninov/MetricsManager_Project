using System;
using System.Collections.Generic;

namespace MetricsAgent.Responses
{
    public class CpuMetricsResponse
    {
        public List<CpuMetricDto> Metrics { get; set; }
    }

    public class CpuMetricDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
