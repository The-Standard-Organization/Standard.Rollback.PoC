using System;

namespace Standard.Rollback.PoC.Brokers.DateTimes
{
    public interface IDateTimeBroker
    {
        DateTimeOffset GetCurrentDateTimeOffset();
    }
}