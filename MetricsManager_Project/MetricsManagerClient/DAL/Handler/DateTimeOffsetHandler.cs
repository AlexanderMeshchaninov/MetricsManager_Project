using System;
using Dapper;
using System.Data;

namespace MetricsManager.DAL
{
    //Хэндлер для парсинга значений в DateTimeOffset если таковые будут в классах моделей
    public class DateTimeOffsetHandler : SqlMapper.TypeHandler<DateTimeOffset>
    {
        public override DateTimeOffset Parse(object value) 
            => DateTimeOffset.FromUnixTimeMilliseconds((long) value);

        public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
            => parameter.Value = value;
    }
}
