//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SeeSql.ApplicationServices
{
    using System;
    using System.Collections.Generic;
    
    public partial class sysjobactivity
    {
        public int session_id { get; set; }
        public System.Guid job_id { get; set; }
        public Nullable<System.DateTime> run_requested_date { get; set; }
        public string run_requested_source { get; set; }
        public Nullable<System.DateTime> queued_date { get; set; }
        public Nullable<System.DateTime> start_execution_date { get; set; }
        public Nullable<int> last_executed_step_id { get; set; }
        public Nullable<System.DateTime> last_executed_step_date { get; set; }
        public Nullable<System.DateTime> stop_execution_date { get; set; }
        public Nullable<int> job_history_id { get; set; }
        public Nullable<System.DateTime> next_scheduled_run_date { get; set; }
    }
}
