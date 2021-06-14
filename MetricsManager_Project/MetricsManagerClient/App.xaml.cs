using System;
using System.Windows;
using AutoMapper;
using FluentMigrator.Runner;
using MetricsManagerClient.Client;
using MetricsManagerClient.DAL;
using MetricsManagerClient.DAL.Interface;
using MetricsManagerClient.Factory;
using MetricsManagerClient.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Polly;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace MetricsManagerClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        private readonly NLog.ILogger _logger;

        public App()
        {
            var serviceCollection = new ServiceCollection();

            _logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            _host = new HostBuilder()
                .ConfigureServices((hostContext, service) =>
                {
                    ConfigureService(service);
                    service.AddSingleton<MainWindow>();
                })
                .ConfigureLogging(logBuilder =>
                {
                    logBuilder.AddDebug();
                    logBuilder.ClearProviders();
                    logBuilder.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()
                .Build();
        }

        public void ConfigureService(IServiceCollection service)
        {
            service.AddSingleton<IGetAllCpuMetricsFromManager, GetAllGetAllCpuMetricsFromManager>();
            service.AddSingleton<IGetAllHddMetricsFromManager, GetAllGetAllHddMetricsFromManager>();
            service.AddSingleton<IGetAllRamMetricsFromManager, GetAllGetAllRamMetricsFromManager>();
            service.AddSingleton<IDbConnection, DbConnectionSource>();

            //Библиотека Quartz
            service.AddSingleton<IJobFactory, SingletonJobFactory>();
            service.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            service.AddHostedService<QuartzHostedService>();

            //Клиент запросов к Менеджеру
            service.AddHttpClient<IClient, ManagerClient>()
                .AddTransientHttpErrorPolicy(p => p
                    .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));
            //cpu
            service.AddSingleton<CpuMetricsJobs>();
            service.AddSingleton(new JobSchedule(
                jobType: typeof(CpuMetricsJobs),
                cronExpression: "0/5 * * * * ?"));
            //ram
            service.AddSingleton<RamMetricsJobs>();
            service.AddSingleton(new JobSchedule(
                jobType: typeof(RamMetricsJobs),
                cronExpression: "0/5 * * * * ?"));
            //hdd
            service.AddSingleton<HddMetricsJobs>();
            service.AddSingleton(new JobSchedule(
                jobType: typeof(HddMetricsJobs),
                cronExpression: "0/5 * * * * ?"));

            //Mapper
            var mapperConfiguration = new MapperConfiguration(mp => mp
                .AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            service.AddSingleton(mapper);

            var dbConnectionSource = new DbConnectionSource();
            var connection = dbConnectionSource.AddConnectionDb(100, true);

            service.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // добавляем поддержку SQLite 
                    .AddSQLite()
                    // устанавливаем строку подключения
                    .WithGlobalConnectionString(connection)
                    // подсказываем где искать классы с миграциями
                    .ScanIn(typeof(App).Assembly).For.Migrations())
                .AddLogging(lb => lb
                    .AddFluentMigratorConsole());
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            using (var serviceScope = _host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                try
                {
                    await _host.StartAsync();

                    _logger.Trace("[BEGIN]-Trace");
                    _logger.Debug("[BEGIN]-Debug");
                    _logger.Info("[BEGIN]-Info");

                    var runner = services.GetRequiredService<IMigrationRunner>();
                    runner.MigrateUp();
                    var mainWindow = services.GetRequiredService<MainWindow>();
                    mainWindow.Show();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }
    }
}
