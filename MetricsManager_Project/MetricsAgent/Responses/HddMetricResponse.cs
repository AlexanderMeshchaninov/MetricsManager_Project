using System;
using System.Collections.Generic;

namespace MetricsAgent.Responses
{
    public class HddMetricResponse
    {
        public List<HddMetricDto> Metrics { get; set; }
    }
    public class HddMetricDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
