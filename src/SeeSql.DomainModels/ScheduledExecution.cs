using System;

namespace SeeSql.DomainModels
{
    public class ScheduledExecution
    {
        public ScheduledExecution(Guid jobId, string jobName, DateTime expectedRunDate, TimeSpan? expectedDuration = null, TimeSpan? expectedRunDurationStandardDeviation = null)
        {
            JobId = jobId;
            JobName = jobName;
            ExpectedRunDate = expectedRunDate;
            ExpectedDuration = expectedDuration;
            ExpectedRunDurationStandardDeviation = expectedRunDurationStandardDeviation;
        }

        public Guid JobId { get; }
        public string JobName { get; }
        public TimeSpan? ExpectedDuration { get; }
        public TimeSpan? ExpectedRunDurationStandardDeviation { get; }
        public DateTime ExpectedRunDate { get; }
    }
}