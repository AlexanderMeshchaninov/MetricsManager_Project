using System;
using System.Linq;
using MetricsManager.Client;
using MetricsManager.DAL;
using Quartz;
using System.Threading.Tasks;
using MetricsManager.Models;
using MetricsManager.Requests;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Jobs
{
    [DisallowConcurrentExecution]
    public class RamMetricsJobs : IJob
    {
        private readonly IRamMetricsAgentsRepository _ramMetricsAgentsRepository;

        private readonly IMetricsAgentClient _agentClient;

        private readonly IAgentsRepository _agentsRepository;

        private readonly ILogger<RamMetricsJobs> _logger;

        public RamMetricsJobs(
            IRamMetricsAgentsRepository ramMetricsAgentsRepository,
            IMetricsAgentClient agentClient,
            IAgentsRepository agentsRepository,
            ILogger<RamMetricsJobs> logger)
        {
            _ramMetricsAgentsRepository = ramMetricsAgentsRepository;

            _agentClient = agentClient;

            _agentsRepository = agentsRepository;

            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                //считываем адреса зарегистрированных агентов с бд менеджера
                var agents = _agentsRepository.Read();

                foreach (var agent in agents)
                {
                    //1 = true, агент включен
                    if (agent.Status == 1)
                    {
                        var lastDate = _ramMetricsAgentsRepository.GetLastTime(agent.AgentId);

                        //cоздание запроса в Metrics Agent
                        var response = _agentClient.GetRamMetrics(new RamMetricsApiRequest
                        {
                            ClientBaseAddress = agent.AgentAddress,

                            FromTime = lastDate,

                            ToTime = DateTimeOffset.UtcNow,

                        }).Metrics;

                        foreach (var metrics in response)
                        {
                            //получение метрик и записывание в бд Metrics Manager
                            _ramMetricsAgentsRepository.Create(new RamMetrics()
                            {
                                //Отметка с какого агента собраны метрики
                                AgentId = agent.AgentId,
                                Value = metrics.Value,
                                Time = metrics.Time,
                            });
                        }
                    }
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