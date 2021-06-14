using System.Collections.Generic;

namespace MetricsManager.DAL
{
    public interface IAgentsRepository<T> where T : class
    {
        void Create(T item);
        List<T> Read();
        void Update(int agentId, int status = 0);
    }
}