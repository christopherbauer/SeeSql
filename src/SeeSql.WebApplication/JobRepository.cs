using System;
using System.Collections.Generic;
using System.Linq;
using SeeSql.ApplicationServices;
using SeeSql.DomainServices;

namespace SeeSql.WebApplication
{
    public class JobRepository : IJobRepository
    {
        private readonly MsdbEntities _entities;

        public JobRepository(MsdbEntities entities)
        {
            _entities = entities;
        }

        public IList<sysjob> GetJobs()
        {
            return _entities.sysjobs.ToList();
        }

        public IList<sysjobhistory> GetJobHistory(Guid jobId)
        {
            return _entities.sysjobhistories.Where(sysjobhistory => sysjobhistory.job_id == jobId).ToList();
            //run status
            /*
             *case run_status when 0 then 'failed'
when 1 then 'Succeded' 
when 2 then 'Retry' 
when 3 then 'Cancelled' 
when 4 then 'In Progress' */
        }

        public sysjobactivity GetJobActivity(Guid jobId, int instanceId)
        {
            return
                _entities.sysjobactivities.Single(
                    sysjobactivity => sysjobactivity.job_history_id == instanceId && sysjobactivity.job_id == jobId);
        }

        public List<sysjobschedule> GetJobSchedules()
        {
            return _entities.sysjobschedules.AsNoTracking().ToList();   /*AsNoTracking very important here because
                                                                            * the msdb table for sysjobschedules has 
                                                                            * no PK and because of this EF will just 
                                                                            * repeatedly return the same row
                                                                            */
        }

        public IList<sysschedule> GetSchedules()
        {
            return _entities.sysschedules.ToList();
        }

        public IList<sysjobhistory> GetJobHistory(IEnumerable<Guid> jobId)
        {
            return
                _entities.sysjobhistories.Where(
                    sysjobhistory => jobId.Any(guid => guid == sysjobhistory.job_id) && sysjobhistory.step_id == 0)
                    .GroupBy(sysjobhistory => sysjobhistory.job_id)
                    .Select(s => s.OrderByDescending(
                        sysjobhistory =>
                            sysjobhistory.run_date + "" +
                            ("0" + sysjobhistory.run_time).Substring(("0" + sysjobhistory.run_time).Length - 6, 6))
                        .Take(10))
                    .SelectMany(e => e)
                    .ToList();
        }

        public IList<sysjobstep> GetJobSteps(Guid jobId)
        {
            return _entities.sysjobsteps.Where(sysjobstep => sysjobstep.job_id == jobId).ToList();
        }
    }
}