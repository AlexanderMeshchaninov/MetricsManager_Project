namespace MetricsManager.DAL
{
    public interface IDbConnection
    {
        string AddConnectionDb(int poolSize = 100, bool pooling = true);
    }
}
