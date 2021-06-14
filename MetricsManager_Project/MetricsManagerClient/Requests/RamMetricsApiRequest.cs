﻿using System;

namespace MetricsManagerClient.Requests
{
    public class RamMetricsApiRequest
    {
        public string ClientBaseAddress { get; set; }
        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
    }
}