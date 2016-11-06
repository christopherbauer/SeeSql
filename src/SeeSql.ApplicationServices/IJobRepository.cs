using System;
using System.Collections.Generic;

namespace SeeSql.ApplicationServices
{
    public interface IJobRepository
    {
        IList<sysjob> GetJobs();
        IList<sysjobhistory> GetJobHistory(Guid jobId);
        sysjobactivity GetJobActivity(Guid jobId, int instanceId);
        List<sysjobschedule> GetJobSchedules();
        IList<sysschedule> GetSchedules();
        IList<sysjobhistory> GetJobHistory(IEnumerable<Guid> jobId);
        IList<sysjobstep> GetJobSteps(Guid jobId);
    }
}