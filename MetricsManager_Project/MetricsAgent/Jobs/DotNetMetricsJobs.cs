using MetricsAgent.DAL;
using Quartz;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using MetricsAgent.Models;

namespace MetricsAgent.Jobs
{
    public class DotNetMetricsJobs : IJob
    {
        private IDotNetMetricsRepository _repository;

        //счетчик для метрик .NET
        private PerformanceCounter _dotNetCounter;

        public DotNetMetricsJobs(IDotNetMetricsRepository repository)
        {
            _repository = repository;

            _dotNetCounter = new PerformanceCounter
            (
                ".NET CLR Memory",
                "# Bytes in all Heaps",
                "_Global_"
            );
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости DotNet
            var dotNetUsageInPercents = Convert.ToInt32(_dotNetCounter.NextValue());

            //узнаем когда сняли значение метрики
            var time = DateTimeOffset.FromUnixTimeMilliseconds(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            //теперь можно записать что-то при помощи репозитория
            _repository.Create(new DotNetMetrics
            {
                Time = time,
                Value = dotNetUsageInPercents,
            });

            return Task.CompletedTask;
        }
    }
}