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
    
    public partial class sysjobhistory
    {
        public int instance_id { get; set; }
        public System.Guid job_id { get; set; }
        public int step_id { get; set; }
        public string step_name { get; set; }
        public int sql_message_id { get; set; }
        public int sql_severity { get; set; }
        public string message { get; set; }
        public int run_status { get; set; }
        public int run_date { get; set; }
        public int run_time { get; set; }
        public int run_duration { get; set; }
        public int operator_id_emailed { get; set; }
        public int operator_id_netsent { get; set; }
        public int operator_id_paged { get; set; }
        public int retries_attempted { get; set; }
        public string server { get; set; }
    }
}
