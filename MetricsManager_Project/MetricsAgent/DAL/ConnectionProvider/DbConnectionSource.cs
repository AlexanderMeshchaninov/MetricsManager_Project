namespace MetricsAgent.DAL
{
    public class DbConnectionSource : IDbConnection
    {
        private string DbConnection { get; set; }

        public string AddConnectionDb(int poolSize, bool pooling = true)
        {
            return DbConnection = @$"Data Source=metrics.db;Version=3;Pooling={pooling.ToString()};Max Pool Size={poolSize.ToString()};";
        }
    }
}