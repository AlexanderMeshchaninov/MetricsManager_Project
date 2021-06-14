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
                    Title = "API - Metrics Agent (����� ����� ��������� ������)",
                    Description = "����� ����� �������������� ������ ������ ����� ������",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Meshchaninov",
                        Email = string.Empty,
                        Url = new Uri("https://gb.ru"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "����� ����� ������� ��� ����� ��������� ��� ������������",
                        Url = new Uri("https://example.com/license"),
                    }

                });
                //��������� ������ ����� xml ������������ (����������� ������������)
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

            //��������� ���� ������

            //cpu
            services.AddSingleton<CpuMetricsJobs>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(CpuMetricsJobs),
                cronExpression: "0/5 * * * * ?")); //������ ������ 5 ���.
            //dotnet
            services.AddSingleton<DotNetMetricsJobs>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(DotNetMetricsJobs),
                cronExpression: "0/5 * * * * ?")); //������ ������ 5 ���.
            //hdd
            services.AddSingleton<HddMetricsJobs>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(HddMetricsJobs),
                cronExpression: "0/5 * * * * ?")); //������ ������ 5 ���.
            //network
            services.AddSingleton<NetworkMetricsJobs>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(NetworkMetricsJobs),
                cronExpression: "0/5 * * * * ?")); //������ ������ 5 ���.
            //ram
            services.AddSingleton<RamMetricsJobs>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(RamMetricsJobs),
                cronExpression: "0/5 * * * * ?")); //������ ������ 5 ���.

            var mapperConfiguration = new MapperConfiguration(mp => mp
                .AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);


            var dbConnectionSource = new DbConnectionSource();
            var connection = dbConnectionSource.AddConnectionDb(100, true);

            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // ��������� ��������� SQLite 
                    .AddSQLite()
                    // ������������� ������ �����������
                    .WithGlobalConnectionString(connection)
                    // ������������ ��� ������ ������ � ����������
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

                // ��������� middleware � �������� ��� ��������� Swagger ��������.
                app.UseSwagger();
                // ��������� middleware ��� ��������� swagger-ui 
                // ��������� Swagger JSON �������� (���� ���������� �� ��������������� �������������
                // �� ������� ����� �������� UI).
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API - Metrics Agent (����� ����� ��������� ������)");

                    //������ ������� ����
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

            // ��������� ��������
            migrationRunner.MigrateUp();
        }
    }
}
