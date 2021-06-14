using System;
using System.Collections.Generic;

namespace MetricsManager.DAL
{
    public interface IRepository<T> where T : class
    {
        void Create(T item);
        IList<T> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime);
        DateTimeOffset GetLastTime(int item);
    }
}
