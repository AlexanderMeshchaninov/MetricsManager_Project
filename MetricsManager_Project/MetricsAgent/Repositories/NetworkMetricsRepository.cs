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
    public interface INetworkMetricsRepository : IRepository<NetworkMetrics>
    {
    }

    public class NetworkMetricsRepository : INetworkMetricsRepository
    {
        private readonly IDbConnection _dbConnection;

        public NetworkMetricsRepository(IDbConnection dbConnection)
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());

            _dbConnection = dbConnection;
        }

        public void Create(NetworkMetrics item)
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                //запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO networkmetrics(Value, Time) VALUES(@Value, @Time)",
                    new
                    {
                        Value = item.Value,
                        Time = item.Time.ToUnixTimeMilliseconds(),
                    });
            }
        }

        public IList<NetworkMetrics> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(_dbConnection.AddConnectionDb()))
            {
                //читаем при помощи Query и в шаблон подставляем тип данных
                //объект которого Dapper сам и заполнит его поля в соответствии с названиями колонок
                return connection.Query<NetworkMetrics>("SELECT * FROM networkmetrics WHERE Time>=@fromTime AND Time<=@toTime",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeMilliseconds(),
                        toTime = toTime.ToUnixTimeMilliseconds()
                    }).ToList();
            }
        }
    }
}
