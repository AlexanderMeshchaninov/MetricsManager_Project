using System;
using System.Collections.Generic;
using MetricsManagerClient.Models;
using Dapper;
using System.Data.SQLite;
using System.Linq;
using MetricsManager.DAL;

namespace MetricsManagerClient.DAL
{
    public interface IGetAllCpuMetricsFromManager : IGetAllMetricsFromManager<CpuMetrics>
    {
    }

    public class GetAllGetAllCpuMetricsFromManager : IGetAllCpuMetricsFromManager
    {
        private readonly IDbConnection _dbConnection;

        public GetAllGetAllCpuMetricsFromManager(IDbConnection dbConnection)
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());

            _dbConnection = dbConnection;
        }

        public void Create(CpuMetrics item)
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                //запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO cpumetrics(AgentId, Value, Time) VALUES(@AgentId, @Value, @Time)",
                    new
                    {
                        AgentId = item.AgentId,
                        Value = item.Value,
                        Time = item.Time.ToUnixTimeMilliseconds()
                    });
            }
        }

        public IList<CpuMetrics> ReadAllMetrics()
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                //запрос на вставку данных с плейсхолдерами для параметров
                return connection.Query<CpuMetrics>("SELECT * FROM cpumetrics LIMIT 20").ToList();
            }
        }

        public DateTimeOffset GetLastTime()
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                return connection.ExecuteScalar<DateTimeOffset>(
                    "SELECT MAX(Time) FROM cpumetrics");
            }
        }

        public void DeleteMetrics()
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                connection.Execute("DELETE FROM cpumetrics");
            }
        }
    }
}
