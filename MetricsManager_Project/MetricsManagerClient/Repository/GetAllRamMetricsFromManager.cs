using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using MetricsManager.DAL;
using MetricsManagerClient.Models;

namespace MetricsManagerClient.DAL
{
    public interface IGetAllRamMetricsFromManager : IGetAllMetricsFromManager<RamMetrics>
    {
    }

    public class GetAllGetAllRamMetricsFromManager : IGetAllRamMetricsFromManager
    {
        private readonly IDbConnection _dbConnection;

        public GetAllGetAllRamMetricsFromManager(IDbConnection dbConnection)
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());

            _dbConnection = dbConnection;
        }

        public void Create(RamMetrics item)
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                //запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO rammetrics(AgentId, Value, Time) VALUES(@AgentId, @Value, @Time)",
                    new
                    {
                        AgentId = item.AgentId,
                        Value = item.Value,
                        Time = item.Time.ToUnixTimeMilliseconds()
                    });
            }
        }

        public IList<RamMetrics> ReadAllMetrics()
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                //запрос на вставку данных с плейсхолдерами для параметров
                return connection.Query<RamMetrics>("SELECT * FROM rammetrics LIMIT 20").ToList();
            }
        }

        public DateTimeOffset GetLastTime()
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                return connection.ExecuteScalar<DateTimeOffset>(
                    "SELECT MAX(Time) FROM rammetrics");
            }
        }

        public void DeleteMetrics()
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                connection.Execute("DELETE FROM rammetrics");
            }
        }
    }
}