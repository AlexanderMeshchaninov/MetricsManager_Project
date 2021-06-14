using MetricsManager.DAL;

namespace MetricsManager
{
    public class DbConnectionSource : IDbConnection
    {
        public string DbConnection { get; set; }

        public DbConnectionSource()
        {
            DbConnection = @"Data Source=metrics.db;Version=3;Pooling=True;Max Pool Size=100;";
        }

        public string AddConnectionDb(int poolSize = 100, bool pooling = true)
        {
            return DbConnection = @$"Data Source=metrics.db;Version=3;Pooling={pooling.ToString()};Max Pool Size={poolSize.ToString()};";
        }
    }
}
