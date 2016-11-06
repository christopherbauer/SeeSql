using System;
using System.Collections.Generic;
using SeeSql.DomainModels;

namespace SeeSql.WebApplication.ViewModels
{
    public class JobSchedulerListViewModel
    {
        public IList<JobSchedule> JobSchedules { get; set; }
        public IList<Job> Jobs { get; set; }
        public IList<RunTimeStatistics> JobRunTimeStatistics { get; set; }
        public List<ScheduledExecution> InterpolatedRunTimes { get; set; }
        public int JobCount { get; set; }
        public DateTime CurrentDateTime { get; set; }
        public DateTime RangeStart { get; set; }
        public DateTime RangeEnd { get; set; }
    }
}