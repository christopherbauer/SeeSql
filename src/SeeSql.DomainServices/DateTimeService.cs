using System;

namespace SeeSql.DomainServices
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime GetCurrentUtcDateTime()
        {
            return DateTime.UtcNow;
        }

        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}