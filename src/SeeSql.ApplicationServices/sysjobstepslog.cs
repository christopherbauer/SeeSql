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
    
    public partial class sysjobstepslog
    {
        public int log_id { get; set; }
        public string log { get; set; }
        public System.DateTime date_created { get; set; }
        public System.DateTime date_modified { get; set; }
        public Nullable<long> log_size { get; set; }
        public System.Guid step_uid { get; set; }
    }
}
