using System;
using System.Collections.Generic;

namespace MetricsAgent.Responses
{
    public class NetworkMetricResponse
    {
        public List<NetworkMetricDto> Metrics { get; set; }
    }
    public class NetworkMetricDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
