using System;
using System.Collections.Generic;
using System.Data.SQLite;
using MetricsAgent.Models;
using Dapper;
using System.Linq;

namespace MetricsAgent.DAL
{
    //маскировочный интерфейс
    //необходим, чтобы проверить работу репозитория на тесте-заглушке
    public interface ICpuMetricsRepository : IRepository<CpuMetrics>
    {
    }

    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private readonly IDbConnection _dbConnection;

        public CpuMetricsRepository(IDbConnection dbConnection)
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());

            _dbConnection = dbConnection;
        }

        public void Create(CpuMetrics item)
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                //запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO cpumetrics(Value, Time) VALUES(@Value, @Time)",
                    new
                    {
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
    }
}
