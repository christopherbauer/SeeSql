using System.Collections.Generic;
using SeeSql.DomainModels;

namespace SeeSql.WebApplication.ViewModels
{
    public class JobScheduleListViewModel
    {
        public IList<JobSchedule> JobSchedules { get; set; }
        public IList<Job> Jobs { get; set; }
    }
}