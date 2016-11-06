using System;

namespace SeeSql.DomainServices
{
    public interface IDateTimeService
    {
        DateTime GetCurrentUtcDateTime();
        DateTime GetCurrentDateTime();
    }
}