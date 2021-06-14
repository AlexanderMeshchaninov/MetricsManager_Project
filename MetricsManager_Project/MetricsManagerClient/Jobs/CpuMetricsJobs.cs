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
    public class CpuMetricsJobs : IJob
    {
        private readonly IClient _agentClient;

        private readonly ILogger<CpuMetricsJobs> _logger;

        private readonly IGetAllCpuMetricsFromManager _allCpuMetrics;

        public CpuMetricsJobs(IClient agentClient, ILogger<CpuMetricsJobs> logger, IGetAllCpuMetricsFromManager allCpuMetrics)
        {
            _agentClient = agentClient;

            _logger = logger;

            _allCpuMetrics = allCpuMetrics;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var lastDate = _allCpuMetrics.GetLastTime();

                var request = new CpuMetricsApiRequest()
                {
                    ClientBaseAddress = "http://localhost:51111",
                        
                    FromTime = lastDate,

                    ToTime = DateTimeOffset.UtcNow,
                };
                
                //cоздание запроса в Metrics Manager
                var response = _agentClient.GetAllCpuMetrics(request).Metrics;

                foreach (var metrics in response)
                {
                    _allCpuMetrics.Create(new CpuMetrics()
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