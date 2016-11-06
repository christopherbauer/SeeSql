using System;

namespace SeeSql.DomainServices
{
    public class SqlConverter
    {
        public static DateTime? GetDateTimeFromInt(int dateInt, int timeInt, DateTimeKind dateTimeKind = DateTimeKind.Unspecified)
        {
            DateTime? runDateTime = null;
            if (dateInt > 0)
            {
                var runDate = dateInt.ToString(); //run date is in YYYYMMDD, but a "0" run date means never run...
                var runTime = timeInt.ToString("000000");//HHMMSS
                runDateTime = new DateTime(Convert.ToInt32(runDate.Substring(0, 4)),
                    Convert.ToInt32(runDate.Substring(4, 2)),
                    Convert.ToInt32(runDate.Substring(6, 2)), Convert.ToInt32(runTime.Substring(0, 2)),
                    Convert.ToInt32(runTime.Substring(2, 2)), Convert.ToInt32(runTime.Substring(4, 2)), dateTimeKind);
            }
            return runDateTime;
        }

        public static TimeSpan GetRunDurationFromInt(int runDuration)
        {
            var runDurationString = runDuration.ToString("000000"); //HHMMSS
            return new TimeSpan(runDuration >= 240000 ? runDuration / 240000 : 0,//hours is the final measure used by sql server, and can be any number
                Convert.ToInt32(runDurationString.Substring(0, runDurationString.Length - 4)), //hours is everything to the left of mm and ss
                Convert.ToInt32(runDurationString.Substring(runDurationString.Length - 4, 2)),
                Convert.ToInt32(runDurationString.Substring(runDurationString.Length - 2, 2)));
        }
    }
}