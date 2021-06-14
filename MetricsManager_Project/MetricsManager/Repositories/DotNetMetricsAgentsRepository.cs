using System;
using System.Collections.Generic;
using System.Data.SQLite;
using MetricsManager.Models;
using Dapper;
using System.Linq;

namespace MetricsManager.DAL
{
    //маскировочный интерфейс
    //необходим, чтобы проверить работу репозитория на тесте-заглушке
    public interface IDotNetMetricsAgentsRepository : IRepository<DotNetMetrics>
    {
    }

    public class DotNetMetricsAgentsRepository : IDotNetMetricsAgentsRepository
    {
        private readonly IDbConnection _dbConnection;

        public DotNetMetricsAgentsRepository(IDbConnection dbConnection)
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());

            _dbConnection = dbConnection;
        }

        public void Create(DotNetMetrics item)
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                //запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO dotnetmetrics(AgentId, Value, Time) VALUES(@AgentId, @Value, @Time)",
                    new
                    {
                        AgentId = item.AgentId,
                        Value = item.Value,
                        Time = item.Time.ToUnixTimeMilliseconds()
                    });
            }
        }

        public IList<DotNetMetrics> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                return connection.Query<DotNetMetrics>("SELECT * FROM dotnetmetrics WHERE Time>=@fromTime AND Time<=@toTime",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeMilliseconds(),
                        toTime = toTime.ToUnixTimeMilliseconds()
                    }).ToList();
            }
        }
        public DateTimeOffset GetLastTime(int agentId)
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                return connection.ExecuteScalar<DateTimeOffset>("SELECT MAX(Time) FROM dotnetmetrics WHERE AgentId=@AgentId",
                    new
                    {
                        AgentId = agentId,
                    });
            }
        }
    }
}
