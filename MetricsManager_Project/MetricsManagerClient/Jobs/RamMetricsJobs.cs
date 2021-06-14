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
    public class RamMetricsJobs : IJob
    {
        private readonly IClient _agentClient;

        private readonly ILogger<RamMetricsJobs> _logger;

        private readonly IGetAllRamMetricsFromManager _allRamMetrics;

        public RamMetricsJobs(IClient agentClient, ILogger<RamMetricsJobs> logger, IGetAllRamMetricsFromManager allRamMetrics)
        {
            _agentClient = agentClient;

            _logger = logger;

            _allRamMetrics = allRamMetrics;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var lastDate = _allRamMetrics.GetLastTime();

                var request = new RamMetricsApiRequest()
                {
                    ClientBaseAddress = "http://localhost:51111",

                    FromTime = lastDate,

                    ToTime = DateTimeOffset.UtcNow,
                };

                //cоздание запроса в Metrics Manager
                var response = _agentClient.GetAllRamMetrics(request).Metrics;

                foreach (var metrics in response)
                {
                    _allRamMetrics.Create(new RamMetrics()
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