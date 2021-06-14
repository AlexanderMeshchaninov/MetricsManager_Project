using MetricsAgent.DAL;
using Quartz;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using MetricsAgent.Models;

namespace MetricsAgent.Jobs
{
    public class RamMetricsJobs : IJob
    {
        private IRamMetricsRepository _repository;

        //счетчик для метрик RAM
        private PerformanceCounter _ramCounter;

        public RamMetricsJobs(IRamMetricsRepository repository)
        {
            _repository = repository;

            _ramCounter = new PerformanceCounter
            (
                "Memory",
                "% Committed Bytes In Use"
            );
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости RAM
            var ramUsageInPercents = Convert.ToInt32(_ramCounter.NextValue());

            //узнаем когда сняли значение метрики
            var time = DateTimeOffset.FromUnixTimeMilliseconds(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            //теперь можно записать что-то при помощи репозитория
            _repository.Create(new RamMetrics
            {
                Time = time,
                Value = ramUsageInPercents,
            });

            return Task.CompletedTask;
        }
    }
}