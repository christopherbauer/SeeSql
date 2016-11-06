using System;

namespace SeeSql.DomainServices
{
    public static class TimeSpanExtensions
    {
        public static bool IsEqualTo(this TimeSpan a, TimeSpan b)
        {
            return a.CompareTo(b) == 0;
        }
        public static bool IsLessThan(this TimeSpan a, TimeSpan b)
        {
            return a.CompareTo(b) == -1;
        }
        public static bool IsLessThanOrEqualTo(this TimeSpan a, TimeSpan b)
        {
            return a.CompareTo(b) != 1;
        }
        public static bool IsGreaterThan(this TimeSpan a, TimeSpan b)
        {
            return a.CompareTo(b) == 1;
        }
        public static bool IsGreaterThanOrEqualTo(this TimeSpan a, TimeSpan b)
        {
            return a.CompareTo(b) != -1;
        }
    }
}