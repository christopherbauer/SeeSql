using System;
using System.Collections.Generic;

namespace SeeSql.DomainModels
{

    public class Job
    {
        public Guid JobId { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public string Description { get; set; }
        public int StartStepId { get; set; }

        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateModified { get; set; }
        public int VersionNumber { get; set; }
        public IList<JobSchedule> Schedules { get; set; }
    }
}