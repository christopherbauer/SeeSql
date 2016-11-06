using System;

namespace SeeSql.DomainModels
{
    public class JobHistory
    {
        public int InstanceId { get; set; } //I believe this is the execution PK
        public System.Guid JobId { get; set; }
        public int StepId { get; set; }
        public string StepName { get; set; }
        public int SqlMessageId { get; set; }
        public int SqlSeverity { get; set; }
        public string Message { get; set; }
        public int RunStatus { get; set; }
        public DateTime? RunDate { get; set; }
        public int RunTime { get; set; }
        public TimeSpan RunDuration { get; set; }
    }
}