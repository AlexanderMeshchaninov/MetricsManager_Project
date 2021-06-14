using System;
using Quartz;
using System.Threading.Tasks;
using MetricsManagerClient.DAL;
using MetricsManagerClient.DAL.Interface;
using MetricsManagerClient.Models;
using MetricsManagerClient.Requests;
using Microsoft.Extensions.Logging;

namespace MetricsManagerClient.Jobs
{
    [DisallowConcurrentExecution]
    public class HddMetricsJobs : IJob
    {
        private readonly IClient _agentClient;

        private readonly ILogger<HddMetricsJobs> _logger;

        private readonly IGetAllHddMetricsFromManager _allHddMetrics;

        public HddMetricsJobs(IClient agentClient, ILogger<HddMetricsJobs> logger, IGetAllHddMetricsFromManager allHddMetrics)
        {
            _agentClient = agentClient;

            _logger = logger;

            _allHddMetrics = allHddMetrics;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var lastDate = _allHddMetrics.GetLastTime();

                var request = new HddMetricsApiRequest()
                {
                    ClientBaseAddress = "http://localhost:51111",

                    FromTime = lastDate,

                    ToTime = DateTimeOffset.UtcNow,
                };

                //cоздание запроса в Metrics Manager
                var response = _agentClient.GetAllHddMetrics(request).Metrics;

                foreach (var metrics in response)
                {
                    _allHddMetrics.Create(new HddMetrics()
                    {
                        AgentId = metrics.AgentId,
                        Value = metrics.Value,
                        Time = metrics.Time,
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Task.CompletedTask;
        }
    }
}