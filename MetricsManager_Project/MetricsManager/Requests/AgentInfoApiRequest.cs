using System;

namespace MetricsManager.Requests
{
    public class AgentInfoApiRequest
    {
        public int AgentId { get; set; }
        public string AgentAddress { get; set; }
    }
}