using MetricsAgent.DAL;
using Quartz;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using MetricsAgent.Models;

namespace MetricsAgent.Jobs
{
    public class NetworkMetricsJobs : IJob
    {
        private INetworkMetricsRepository _repository;

        //счетчик для метрик NETWORK
        private PerformanceCounter _networkCounter;

        public NetworkMetricsJobs(INetworkMetricsRepository repository)
        {
            _repository = repository;

            _networkCounter = new PerformanceCounter
            (
                "Network Interface",
                "Bytes Total/sec",
                "Broadcom 802.11ac Network Adapter"
            );
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости Network
            var networkUsageInPercents = Convert.ToInt32(_networkCounter.NextValue());

            //узнаем когда сняли значение метрики
            var time = DateTimeOffset.FromUnixTimeMilliseconds(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            //теперь можно записать что-то при помощи репозитория
            _repository.Create(new NetworkMetrics
            {
                Time = time,
                Value = networkUsageInPercents,
            });

            return Task.CompletedTask;
        }
    }
}