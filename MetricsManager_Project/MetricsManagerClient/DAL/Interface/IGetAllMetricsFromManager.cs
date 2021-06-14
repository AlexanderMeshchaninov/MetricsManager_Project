using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace MetricsManagerClient.DAL
{
    public interface IGetAllMetricsFromManager<T> where T : class
    {
        void Create(T item);
        IList<T> ReadAllMetrics();

        DateTimeOffset GetLastTime();

        void DeleteMetrics();
    }
}
