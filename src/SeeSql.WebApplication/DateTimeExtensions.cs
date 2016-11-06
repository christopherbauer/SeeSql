using System;

namespace SeeSql.WebApplication
{
    public static class DateTimeExtensions
    {
        public static string ToJavascriptString(this DateTime value)
        {
            var month = value.Month - 1;
            return string.Format("{0}{1}{2}", value.ToString("yyyy,"), month, value.ToString(",d,H,m,s"));
        }
    }
}