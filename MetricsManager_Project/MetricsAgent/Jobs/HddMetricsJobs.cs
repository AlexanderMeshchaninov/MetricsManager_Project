using MetricsAgent.DAL;
using Quartz;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using MetricsAgent.Models;

namespace MetricsAgent.Jobs
{
    public class HddMetricsJobs : IJob
    {
        private IHddMetricsRepository _repository;

        //счетчик для метрик HDD
        private PerformanceCounter _hddCounter;

        public HddMetricsJobs(IHddMetricsRepository repository)
        {
            _repository = repository;

            _hddCounter = new PerformanceCounter
            (
                "PhysicalDisk",
                "% Disk Time",
                "_Total"
            );
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости Hdd
            var hddUsageInPercents = Convert.ToInt32(_hddCounter.NextValue());

            //узнаем когда сняли значение метрики
            var time = DateTimeOffset.FromUnixTimeMilliseconds(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            //теперь можно записать что-то при помощи репозитория
            _repository.Create(new HddMetrics
            {
                Time = time,
                Value = hddUsageInPercents,
            });

            return Task.CompletedTask;
        }
    }
}