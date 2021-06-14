using System;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
    public class NetworkMetricsApiResponse
    {
        public List<NetworkMetricDto> Metrics { get; set; }
    }
    public class NetworkMetricDto
    {
        public int AgentId { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}