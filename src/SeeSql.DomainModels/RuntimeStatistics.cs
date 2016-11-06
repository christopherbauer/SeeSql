using System;

namespace SeeSql.DomainModels
{
    public class RunTimeStatistics
    {
        public Guid JobId { get; set; }
        public string JobName { get; set; }
        public TimeSpan AverageDuration { get; set; }
        public TimeSpan MaxDuration { get; set; }
        public TimeSpan RunDurationStandardDeviation { get; set; }
        public DateTime NextRunDate { get; set; }
        public string Name { get; set; }
    }
}
