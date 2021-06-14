using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using MetricsManager.DAL;
using MetricsManagerClient.Models;

namespace MetricsManagerClient.DAL
{
    public interface IGetAllHddMetricsFromManager : IGetAllMetricsFromManager<HddMetrics>
    {
    }

    public class GetAllGetAllHddMetricsFromManager : IGetAllHddMetricsFromManager
    {
        private readonly IDbConnection _dbConnection;

        public GetAllGetAllHddMetricsFromManager(IDbConnection dbConnection)
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());

            _dbConnection = dbConnection;
        }

        public void Create(HddMetrics item)
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                //запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO hddmetrics(AgentId, Value, Time) VALUES(@AgentId, @Value, @Time)",
                    new
                    {
                        AgentId = item.AgentId,
                        Value = item.Value,
                        Time = item.Time.ToUnixTimeMilliseconds()
                    });
            }
        }

        public IList<HddMetrics> ReadAllMetrics()
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                //запрос на вставку данных с плейсхолдерами для параметров
                return connection.Query<HddMetrics>("SELECT * FROM hddmetrics LIMIT 20").ToList();
            }
        }

        public DateTimeOffset GetLastTime()
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                return connection.ExecuteScalar<DateTimeOffset>(
                    "SELECT MAX(Time) FROM hddmetrics");
            }
        }

        public void DeleteMetrics()
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                connection.Execute("DELETE FROM hddmetrics");
            }
        }
    }
}