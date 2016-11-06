using System;

namespace SeeSql.DomainModels
{
    public class SystemJobSchedule
    {
        public Guid JobId { get; set; }
        public int? ScheduleId { get; set; }
        public DateTime NextRunDate { get; set; }
    }
}