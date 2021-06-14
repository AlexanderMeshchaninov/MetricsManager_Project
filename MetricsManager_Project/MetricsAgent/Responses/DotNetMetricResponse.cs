using System;
using System.Collections.Generic;

namespace MetricsAgent.Responses
{
    public class DotNetMetricResponse
    {
        public List<DotNetMetricDto> Metrics { get; set; }
    }
    public class DotNetMetricDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
