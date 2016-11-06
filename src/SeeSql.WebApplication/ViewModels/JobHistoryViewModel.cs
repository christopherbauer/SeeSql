using System;
using System.Collections.Generic;
using SeeSql.DomainModels;

namespace SeeSql.WebApplication.ViewModels
{
    public class JobHistoryViewModel
    {
        public IList<JobHistory> JobHistory { get; set; }
        public Guid JobId { get; set; }
        public int InstanceId { get; set; }
    }
}