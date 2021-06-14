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
    public interface ICpuMetricsAgentsRepository : IRepository<CpuMetrics>
    {
    }

    public class CpuMetricsAgentsRepository : ICpuMetricsAgentsRepository
    {
        private readonly IDbConnection _dbConnection;

        public CpuMetricsAgentsRepository(IDbConnection dbConnection)
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

        public IList<CpuMetrics> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                return connection.Query<CpuMetrics>("SELECT * FROM cpumetrics WHERE Time>=@fromTime AND Time<=@toTime",
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
                return connection.ExecuteScalar<DateTimeOffset>("SELECT MAX(Time) FROM cpumetrics WHERE AgentId=@AgentId",
                new
                    {
                        AgentId = agentId,
                    });
            }
        }
    }
}
