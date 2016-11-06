using System;
using System.Collections.Generic;

namespace SeeSql.DomainModels
{
    public class JobSchedule
    {
        public Guid JobId { get; set; }
        public int? ScheduleId { get; set; }
        public DateTime NextRunDate { get; set; }
        public string Name { get; set; }
        //        public byte[] owner_sid { get; set; }
        public bool Enabled { get; set; }
        public string FrequencyFriendlyText
        {
            get
            {

                string frequencySubdayTypeBase = string.Empty;
                if (FreqSubdayType == 1)
                {
                    frequencySubdayTypeBase = "At the specified time";
                }
                else if (FreqSubdayType == 2)
                {
                    frequencySubdayTypeBase = "Seconds";
                }
                else if (FreqSubdayType == 4)
                {
                    frequencySubdayTypeBase = "Minutes";
                }
                else if (FreqSubdayType == 8)
                {
                    frequencySubdayTypeBase = "Hours";
                }

                string frequencyBase = string.Empty;
                if (FreqType == 1) //Once
                {
                    frequencyBase = string.Format("Once at {0}", ActiveStartDate.ToShortTimeString());
                }
                else if (FreqType == 4) //Daily
                {
                    frequencyBase = string.Format("Daily every {0} {1} from {2} to {3}", FreqInterval, frequencySubdayTypeBase, ActiveStartDate.ToShortTimeString(), ActiveEndDate.ToShortTimeString());
                }
                else if (FreqType == 8) //Weekly
                {
                    var dayOfWeek = new List<string>();
                    var inProcFrequencyInterval = FreqInterval;
                    if (inProcFrequencyInterval >= 64)
                    {
                        dayOfWeek.Add("Saturday");
                        inProcFrequencyInterval -= 64;
                    }
                    if (inProcFrequencyInterval >= 32)
                    {
                        dayOfWeek.Add("Friday");
                        inProcFrequencyInterval -= 32;
                    }
                    if (inProcFrequencyInterval >= 16)
                    {
                        dayOfWeek.Add("Thursday");
                        inProcFrequencyInterval -= 16;
                    }
                    if (inProcFrequencyInterval >= 8)
                    {
                        dayOfWeek.Add("Wednesday");
                        inProcFrequencyInterval -= 8;
                    }
                    if (inProcFrequencyInterval >= 4)
                    {
                        dayOfWeek.Add("Tuesday");
                        inProcFrequencyInterval -= 4;
                    }
                    if (inProcFrequencyInterval >= 2)
                    {
                        dayOfWeek.Add("Monday");
                        inProcFrequencyInterval -= 2;
                    }
                    if (inProcFrequencyInterval >= 1)
                    {
                        dayOfWeek.Add("Sunday");
                        inProcFrequencyInterval -= 1;
                    }
                    if (inProcFrequencyInterval > 0)
                    {
                        throw new ApplicationException("I did something wrong");
                    }
                    dayOfWeek.Reverse();
                    if (FreqSubdayType == 1) //At the specified time...
                    {
                        frequencyBase = string.Format("{0} at {1}", string.Join(", ", dayOfWeek),
                            ActiveStartDate.ToShortTimeString());
                    }
                    else
                    {
                        frequencyBase = string.Format("{0} every {1} {2} starting at {3}", string.Join(", ", dayOfWeek),
                            FreqSubdayInterval, frequencySubdayTypeBase, ActiveStartDate.ToShortTimeString());
                    }
                }
                else if (FreqType == 16) //Monthly
                {
                    frequencyBase = string.Format("Monthly on {0} day of the month", FreqInterval);
                }
                else if (FreqType == 32) //Monthly, relative
                {

                    string dayOfMonth = string.Empty;
                    if (FreqInterval == 1)
                    {
                        dayOfMonth = "Sunday";
                    }
                    else if (FreqInterval == 2)
                    {
                        dayOfMonth = "Monday";
                    }
                    else if (FreqInterval == 3)
                    {
                        dayOfMonth = "Tuesday";
                    }
                    else if (FreqInterval == 4)
                    {
                        dayOfMonth = "Wednesday";
                    }
                    else if (FreqInterval == 5)
                    {
                        dayOfMonth = "Thursday";
                    }
                    else if (FreqInterval == 6)
                    {
                        dayOfMonth = "Friday";
                    }
                    else if (FreqInterval == 7)
                    {
                        dayOfMonth = "Saturday";
                    }
                    else if (FreqInterval == 8)
                    {
                        dayOfMonth = "Day";
                    }
                    else if (FreqInterval == 9)
                    {
                        dayOfMonth = "Weekday";
                    }
                    else if (FreqInterval == 10)
                    {
                        dayOfMonth = "Weekend Day";
                    }
                    else
                    {
                        throw new NotImplementedException("Freq interval not expected");
                    }
                    frequencyBase = string.Format("Monthly on {0}s", dayOfMonth);
                }
                else if (FreqType == 64) //When sql server agent starts
                {
                    frequencyBase = "When sql server agent service starts";
                }
                else if (FreqType == 128) //When the computer is idle
                {
                    frequencyBase = "Runs when the computer is idle";
                }
                else
                {
                    throw new NotImplementedException();
                }


                string frequencyRelativeIntervalBase = string.Empty;
                if (FreqRelativeInterval == 0)
                {
                    frequencyRelativeIntervalBase = "Unused";
                }
                else if (FreqRelativeInterval == 1)
                {
                    frequencyRelativeIntervalBase = "First";
                }
                else if (FreqRelativeInterval == 2)
                {
                    frequencyRelativeIntervalBase = "Second";
                }
                else if (FreqRelativeInterval == 4)
                {
                    frequencyRelativeIntervalBase = "Third";
                }
                else if (FreqRelativeInterval == 8)
                {
                    frequencyRelativeIntervalBase = "Fourth";
                }
                else if (FreqRelativeInterval == 16)
                {
                    frequencyRelativeIntervalBase = "Last";
                }

                return frequencyBase;
            }
        }
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