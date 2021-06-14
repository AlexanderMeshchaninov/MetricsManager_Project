using MetricsAgent.DAL;
using Quartz;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using MetricsAgent.Models;

namespace MetricsAgent.Jobs
{
    public class CpuMetricsJobs : IJob
    {
        private ICpuMetricsRepository _repository;

        //счетчик для метрик CPU
        private PerformanceCounter _cpuCounter;

        public CpuMetricsJobs(ICpuMetricsRepository repository)
        {
            _repository = repository;

            _cpuCounter = new PerformanceCounter
                (
                "Processor", 
                "% Processor Time", 
                "_Total"
                );
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости CPU
            var cpuUsageInPercents = Convert.ToInt32(_cpuCounter.NextValue());

            //узнаем когда сняли значение метрики
            var time = DateTimeOffset.FromUnixTimeMilliseconds(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            //теперь можно записать что-то при помощи репозитория
            _repository.Create(new CpuMetrics
            {
                Time = time,
                Value = cpuUsageInPercents,
            });

            return Task.CompletedTask;
        }
    }
}