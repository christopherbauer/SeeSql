using System;
using System.Collections.Generic;
using SeeSql.DomainModels;

namespace SeeSql.DomainServices
{
    public interface IJobDetailService
    {
        IList<Job> GetJobs();
        IList<JobHistory> GetJobHistoryRollup(Guid jobId);
        IList<JobHistory> GetJobHistory(Guid jobId, int instanceId);
        IList<JobSchedule> GetJobSchedules();
        IList<RunTimeStatistics> GetRunTimeStatistics(IList<Job> jobs, DateTime minDateTime);
        IList<ScheduledExecution> GetInterpolatedRunTimes(IList<Job> jobs, DateTime rangeStart, DateTime rangeEnd);
        JobStep GetJobStep(Guid jobId, int stepId);
    }
}