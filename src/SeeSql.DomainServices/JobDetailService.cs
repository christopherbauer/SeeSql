using System;
using System.Collections.Generic;
using System.Linq;
using SeeSql.ApplicationServices;
using SeeSql.DomainModels;

namespace SeeSql.DomainServices
{
    public class JobDetailService : IJobDetailService
    {
        private readonly IDataMapper _dataMapper;
        private readonly IJobRepository _jobRepository;

        public JobDetailService(IDataMapper dataMapper, IJobRepository jobRepository)
        {
            _dataMapper = dataMapper;
            _jobRepository = jobRepository;
        }

        public IList<Job> GetJobs()
        {
            return _dataMapper.Map<List<Job>>(_jobRepository.GetJobs());
        }

        public IList<JobHistory> GetJobHistoryRollup(Guid jobId)
        {
            return
                _dataMapper.Map<List<JobHistory>>(
                    _jobRepository.GetJobHistory(jobId).Where(sysjobhistory => sysjobhistory.step_id == 0));
        }

        public IList<JobHistory> GetJobHistory(Guid jobId, int instanceId)
        {
            var results = GetJobHistoryRollup(jobId);
            var target = results.Single(history => history.InstanceId == instanceId);
            if (!results.Any(history => history.InstanceId < target.InstanceId && history.StepId == 0))
                //if this is the first ever execution
            {
                return _dataMapper.Map<List<JobHistory>>(
                    _jobRepository.GetJobHistory(jobId).Where(sysjobhistory =>
                            sysjobhistory.step_id > 0 && sysjobhistory.instance_id < target.InstanceId));
            }

            var nextResult =
                results.OrderByDescending(history => history.InstanceId)
                    .First(history => history.InstanceId < target.InstanceId && history.StepId == 0);
            return
                _dataMapper.Map<List<JobHistory>>(
                    _jobRepository.GetJobHistory(jobId).Where(sysjobhistory =>
                        sysjobhistory.step_id > 0 && sysjobhistory.instance_id < target.InstanceId &&
                        sysjobhistory.instance_id > nextResult.InstanceId));
        }

        public IList<JobSchedule> GetJobSchedules()
        {
            var jobs = GetJobs();
            var systemJobSchedules =
                _dataMapper.Map<IList<SystemJobSchedule>>(
                    _jobRepository.GetJobSchedules()
                        .Where(sysjobschedule => sysjobschedule.job_id != null && sysjobschedule.schedule_id != null));
            var systemSchedules = _dataMapper.Map<IList<SystemSchedule>>(_jobRepository.GetSchedules());
            foreach (var job in jobs)
            {
                job.Schedules =
                    _dataMapper.Map<IList<JobSchedule>>(
                        systemJobSchedules.Where(sysjobschedule => sysjobschedule.JobId == job.JobId));

                for (int index = 0; index < job.Schedules.Count; index++)
                {
                    job.Schedules[index] =
                        _dataMapper.Project(
                            systemSchedules.Single(
                                sysschedule => sysschedule.ScheduleId == job.Schedules[index].ScheduleId),
                            job.Schedules[index]);
                }
            }

            return jobs.SelectMany(job => job.Schedules).ToList();
        }

        public IList<RunTimeStatistics> GetRunTimeStatistics(IList<Job> jobs, DateTime minDateTime)
        {
            var jobRunDuration = _jobRepository.GetJobHistory(jobs.Select(job => job.JobId));
            var jobRunTimeStatistics = new List<RunTimeStatistics>();
            var systemJobSchedules =
                _dataMapper.Map<IList<SystemJobSchedule>>(
                    _jobRepository.GetJobSchedules()
                        .Where(sysjobschedule => sysjobschedule.job_id != null && sysjobschedule.schedule_id != null));
            foreach (var job in jobs)
            {
                var workingDurations = jobRunDuration.Where(sysjobhistory => sysjobhistory.job_id == job.JobId);
                if (!workingDurations.Any() ||
                    !systemJobSchedules.Any(
                        schedule => schedule.JobId == job.JobId && schedule.NextRunDate >= minDateTime))
                {
                    continue;
                }
                var statistics = new RunTimeStatistics
                {
                    JobId = job.JobId,
                    JobName = job.Name,
                    AverageDuration =
                        SqlConverter.GetRunDurationFromInt(
                            (int) workingDurations.Average(sysjobhistory => sysjobhistory.run_duration)),
                    MaxDuration =
                        SqlConverter.GetRunDurationFromInt(
                            workingDurations.Max(sysjobhistory => sysjobhistory.run_duration)),
                    NextRunDate = systemJobSchedules.Single(sysschedule => sysschedule.JobId == job.JobId).NextRunDate
                };
                var runDurationStandardDeviation = StandardDeviation(workingDurations,
                    sysjobhistory => sysjobhistory.run_duration);
                if (runDurationStandardDeviation.HasValue)
                {
                    statistics.RunDurationStandardDeviation =
                        SqlConverter.GetRunDurationFromInt((int) runDurationStandardDeviation.Value);
                }

                jobRunTimeStatistics.Add(statistics);
            }

            return jobRunTimeStatistics;
        }

        public IList<ScheduledExecution> GetInterpolatedRunTimes(IList<Job> jobs, DateTime rangeStart, DateTime rangeEnd)
        {
            var systemJobSchedules =
                _dataMapper.Map<IList<SystemJobSchedule>>(
                    _jobRepository.GetJobSchedules()
                        .Where(sysjobschedule => sysjobschedule.job_id != null && sysjobschedule.schedule_id != null));
            var systemSchedules = _dataMapper.Map<IList<SystemSchedule>>(_jobRepository.GetSchedules());
            var scheduledExecutions = new List<ScheduledExecution>();
            var jobRunDuration = _jobRepository.GetJobHistory(jobs.Select(job => job.JobId));
            foreach (var job in jobs)
            {
                var schedules = systemJobSchedules.Where(schedule => schedule.JobId == job.JobId);
                var workingDurations = jobRunDuration.Where(sysjobhistory => sysjobhistory.job_id == job.JobId).ToList();

                foreach (var systemJobSchedule in schedules)
                {
                    var scheduleDetail =
                        systemSchedules.Single(sysschedule => sysschedule.ScheduleId == systemJobSchedule.ScheduleId);
                    if (systemJobSchedule.NextRunDate < rangeStart || systemJobSchedule.NextRunDate > rangeEnd)
                        continue;

                    if (scheduleDetail.FreqType == (int) FrequencyType.Once)
                    {
                        scheduledExecutions.Add(GetScheduledExecution(rangeStart, job, systemJobSchedules,
                            workingDurations));
                    }
                    else if (scheduleDetail.FreqType == (int) FrequencyType.Daily)
                    {
                        for (var inProcessDay = rangeStart;
                            inProcessDay <= rangeEnd;
                            inProcessDay = inProcessDay.AddDays(1))
                        {
                            ProcessDay(rangeStart, rangeEnd, inProcessDay, scheduleDetail, scheduledExecutions, job,
                                systemJobSchedules, workingDurations);
                        }
                    }
                    else if (scheduleDetail.FreqType == (int) FrequencyType.Weekly)
                    {

                        var daysOfweek = new List<DayOfWeek>();
                        var inProcFrequencyInterval = scheduleDetail.FreqInterval;
                        if (inProcFrequencyInterval >= 64)
                        {
                            daysOfweek.Add(DayOfWeek.Saturday);
                            inProcFrequencyInterval -= 64;
                        }
                        if (inProcFrequencyInterval >= 32)
                        {
                            daysOfweek.Add(DayOfWeek.Friday);
                            inProcFrequencyInterval -= 32;
                        }
                        if (inProcFrequencyInterval >= 16)
                        {
                            daysOfweek.Add(DayOfWeek.Thursday);
                            inProcFrequencyInterval -= 16;
                        }
                        if (inProcFrequencyInterval >= 8)
                        {
                            daysOfweek.Add(DayOfWeek.Wednesday);
                            inProcFrequencyInterval -= 8;
                        }
                        if (inProcFrequencyInterval >= 4)
                        {
                            daysOfweek.Add(DayOfWeek.Tuesday);
                            inProcFrequencyInterval -= 4;
                        }
                        if (inProcFrequencyInterval >= 2)
                        {
                            daysOfweek.Add(DayOfWeek.Monday);
                            inProcFrequencyInterval -= 2;
                        }
                        if (inProcFrequencyInterval >= 1)
                        {
                            daysOfweek.Add(DayOfWeek.Sunday);
                            inProcFrequencyInterval -= 1;
                        }
                        if (inProcFrequencyInterval > 0)
                        {
                            throw new ApplicationException("I did something wrong");
                        }

                        for (var inProcessDay = rangeStart;
                            inProcessDay <= rangeEnd;
                            inProcessDay = inProcessDay.AddDays(1))
                        {
                            if (daysOfweek.Any(dayOfWeek => dayOfWeek == inProcessDay.DayOfWeek))
                            {
                                ProcessDay(rangeStart, rangeEnd, inProcessDay, scheduleDetail, scheduledExecutions, job,
                                    systemJobSchedules, workingDurations);
                            }
                        }
                    }
                }
            }
            return scheduledExecutions;
        }

        private void ProcessDay(DateTime rangeStart, DateTime rangeEnd, DateTime inProcessDay,
            SystemSchedule scheduleDetail,
            List<ScheduledExecution> scheduledExecutions, Job job, IList<SystemJobSchedule> systemJobSchedules,
            List<sysjobhistory> workingDurations)
        {
            var dayStartTime = new DateTime(inProcessDay.Year, inProcessDay.Month,
                inProcessDay.Day, scheduleDetail.ActiveStartDate.Hour, scheduleDetail.ActiveStartDate.Minute,
                scheduleDetail.ActiveStartDate.Second);


            var dayEndTime = new DateTime(inProcessDay.Year, inProcessDay.Month,
                inProcessDay.Day, scheduleDetail.ActiveEndDate.Hour, scheduleDetail.ActiveEndDate.Minute,
                scheduleDetail.ActiveEndDate.Second);


            if (scheduleDetail.FreqSubdayType == (int) FrequencySubdayType.AtTheSpecifiedTime)
            {
                scheduledExecutions.Add(GetScheduledExecution(dayStartTime, job, systemJobSchedules, workingDurations));
            }
            else if (scheduleDetail.FreqSubdayType == (int) FrequencySubdayType.Hours)
            {
                for (var hourDateTime = dayStartTime;
                    hourDateTime <= dayEndTime;
                    hourDateTime = hourDateTime.AddHours(scheduleDetail.FreqSubdayInterval))
                {
                    if (hourDateTime > rangeStart && hourDateTime < rangeEnd)
                    {
                        scheduledExecutions.Add(GetScheduledExecution(hourDateTime, job,
                            systemJobSchedules, workingDurations));
                    }
                }
            }
            else if (scheduleDetail.FreqSubdayType == (int) FrequencySubdayType.Minutes)
            {
                for (var minuteDateTime = dayStartTime;
                    minuteDateTime <= dayEndTime;
                    minuteDateTime = minuteDateTime.AddMinutes(scheduleDetail.FreqSubdayInterval))
                {
                    if (minuteDateTime > rangeStart && minuteDateTime < rangeEnd)
                    {
                        scheduledExecutions.Add(GetScheduledExecution(minuteDateTime, job,
                            systemJobSchedules, workingDurations));
                    }
                }
            }
            else if (scheduleDetail.FreqSubdayType == (int) FrequencySubdayType.Seconds)
            {
                for (var secondDateTime = dayStartTime;
                    secondDateTime <= dayEndTime;
                    secondDateTime = secondDateTime.AddSeconds(scheduleDetail.FreqSubdayInterval))
                {
                    if (secondDateTime > rangeStart && secondDateTime < rangeEnd)
                    {
                        scheduledExecutions.Add(GetScheduledExecution(secondDateTime, job,
                            systemJobSchedules, workingDurations));
                    }
                }
            }
        }

        private ScheduledExecution GetScheduledExecution(DateTime executionDateTime, Job job,
            IList<SystemJobSchedule> systemJobSchedules, List<sysjobhistory> workingDurations)
        {
            if (!workingDurations.Any() ||
                !systemJobSchedules.Any(
                    schedule => schedule.JobId == job.JobId && schedule.NextRunDate >= executionDateTime))
            {
                return new ScheduledExecution(job.JobId, job.Name, executionDateTime);
            }
            else
            {
                var standardDeviation = StandardDeviation(workingDurations, sysjobhistory => sysjobhistory.run_duration);
                if (!standardDeviation.HasValue)
                {
                    return new ScheduledExecution(job.JobId, job.Name,
                        executionDateTime,
                        SqlConverter.GetRunDurationFromInt(
                            (int) workingDurations.Average(sysjobhistory => sysjobhistory.run_duration)));
                }
                return new ScheduledExecution(job.JobId, job.Name,
                    executionDateTime,
                    SqlConverter.GetRunDurationFromInt(
                        (int) workingDurations.Average(sysjobhistory => sysjobhistory.run_duration)),
                    SqlConverter.GetRunDurationFromInt(
                        (int)
                        standardDeviation
                            .Value));
            }
        }

        private enum FrequencyType
        {
            Once = 1,
            Daily = 4,
            Weekly = 8
        }

        private enum FrequencySubdayType
        {
            AtTheSpecifiedTime = 1,
            Seconds = 2,
            Minutes = 4,
            Hours = 8
        }

        private double? StandardDeviation<T>(IEnumerable<T> items, Func<T, double> valueFunc)
        {
            if (items == null || !items.Any()) return null;

            var averageDuration = items.Average(valueFunc);
            var deviations =
                items.Select(
                    item => Math.Pow(averageDuration - valueFunc(item), 2));
            var variance = deviations.Average();
            var standardDeviation = Math.Sqrt(variance);
            return standardDeviation;
        }

        public JobStep GetJobStep(Guid jobId, int stepId)
        {
            return _dataMapper.Map<JobStep>(_jobRepository.GetJobSteps(jobId).Single(sysjobstep => sysjobstep.step_id == stepId));
        }

    }
}