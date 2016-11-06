using System;

namespace SeeSql.DomainModels
{
    public class SystemSchedule
    {
        public int? ScheduleId { get; set; }
        public string Name { get; set; }
        //        public byte[] owner_sid { get; set; }
        public bool Enabled { get; set; }
        public int FreqType { get; set; }
        public int FreqInterval { get; set; }
        public int FreqSubdayType { get; set; }
        public int FreqSubdayInterval { get; set; }
        public int FreqRelativeInterval { get; set; }
        public int FreqRecurrenceFactor { get; set; }
        public DateTime ActiveStartDate { get; set; }
        public DateTime ActiveEndDate { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateModified { get; set; }
        public int VersionNumber { get; set; }
    }
}