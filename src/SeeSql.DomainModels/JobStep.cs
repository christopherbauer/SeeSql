using System;

namespace SeeSql.DomainModels
{
    public class JobStep
    {
        public System.Guid JobId { get; set; }
        public int StepId { get; set; }
        public string StepName { get; set; }
        public string SubSystem { get; set; }
        public string Command { get; set; }
        public int Flags { get; set; }
        public string AdditionalParameters { get; set; }
        public int CmdExecSuccessCode { get; set; }
        public byte OnSuccessAction { get; set; }
        public int OnSuccessStepId { get; set; }
        public byte OnFailAction { get; set; }
        public int OnFailStepId { get; set; }
        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseUserName { get; set; }
        public int RetryAttempts { get; set; }
        public int RetryInterval { get; set; }
        public int OSRunPriority { get; set; }
        public string OutputFileName { get; set; }
        public int LastRunOutcome { get; set; }
        public int LastRunDuration { get; set; }
        public int LastRunRetries { get; set; }
        public int LastRunDate { get; set; }
        public int LastRunTime { get; set; }
        public Nullable<int> ProxyId { get; set; }
        public Nullable<System.Guid> StepUId { get; set; }

    }
}