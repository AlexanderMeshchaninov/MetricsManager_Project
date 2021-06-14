using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using MetricsAgent.DAL;
using FluentMigrator.Runner;
using MetricsAgent.Factory;
using MetricsAgent.Jobs;
using Microsoft.OpenApi.Models;
using Quartz.Spi;
using Quartz;
using Quartz.Impl;

namespace MetricsAgent
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API - Metrics Agent (Агент сбора системных метрик)",
                    Description = "Здесь можно протестировать работу Агента сбора метрик",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Meshchaninov",
                        Email = string.Empty,
                        Url = new Uri("https://gb.ru"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Здесь можно указать под какой лицензией все опубликовано",
                        Url = new Uri("https://example.com/license"),
                    }

                });
                //Указываем откуда брать xml документацию (комментарии контроллеров)
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();
            services.AddSingleton<IDotNetMetricsRepository, DotNetMetricsRepository>();
            services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();
            services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();
            services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();
            services.AddSingleton<IDbConnection, DbConnectionSource>();
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddHostedService<QuartzHostedService>();

            //добавляем нашу задачу

            //cpu
            services.AddSingleton<CpuMetricsJobs>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(CpuMetricsJobs),
                cronExpression: "0/5 * * * * ?")); //запуск каждые 5 сек.
            //dotnet
            services.AddSingleton<DotNetMetricsJobs>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(DotNetMetricsJobs),
                cronExpression: "0/5 * * * * ?")); //запуск каждые 5 сек.
            //hdd
            services.AddSingleton<HddMetricsJobs>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(HddMetricsJobs),
                cronExpression: "0/5 * * * * ?")); //запуск каждые 5 сек.
            //network
            services.AddSingleton<NetworkMetricsJobs>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(NetworkMetricsJobs),
                cronExpression: "0/5 * * * * ?")); //запуск каждые 5 сек.
            //ram
            services.AddSingleton<RamMetricsJobs>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(RamMetricsJobs),
                cronExpression: "0/5 * * * * ?")); //запуск каждые 5 сек.

            var mapperConfiguration = new MapperConfiguration(mp => mp
                .AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);


            var dbConnectionSource = new DbConnectionSource();
            var connection = dbConnectionSource.AddConnectionDb(100, true);

            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // добавляем поддержку SQLite 
                    .AddSQLite()
                    // устанавливаем строку подключения
                    .WithGlobalConnectionString(connection)
                    // подсказываем где искать классы с миграциями
                    .ScanIn(typeof(Startup).Assembly).For.Migrations())
                    .AddLogging(lb => lb
                    .AddFluentMigratorConsole());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Включение middleware в пайплайн для обработки Swagger запросов.
                app.UseSwagger();
                // включение middleware для генерации swagger-ui 
                // указываем Swagger JSON эндпоинт (куда обращаться за сгенерированной спецификацией
                // по которой будет построен UI).
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API - Metrics Agent (Агент сбора системных метрик)");

                    //Убрать префикт пути
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // запускаем миграции
            migrationRunner.MigrateUp();
        }
    }
}
