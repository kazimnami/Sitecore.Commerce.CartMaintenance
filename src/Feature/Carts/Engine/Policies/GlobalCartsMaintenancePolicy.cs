using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;

namespace Feature.Carts.Engine
{
    public class GlobalCartsMaintenancePolicy : Policy
    {
        public GlobalCartsMaintenancePolicy()
        {
            DaysToRetainCarts = 30;

            //AllowedSchedule = new List<Schedule>()
            //{
            //    new Schedule { StartTime = "23:00:00", EndTime = "1.01:00:00" }
            //};
        }

        public int DaysToRetainCarts { get; set; }

        public List<Schedule> AllowedSchedule { get; set; }
    }

    public class Schedule
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public TimeSpan GetStartTime()
        {
            return TimeSpan.Parse(StartTime);
        }
        public TimeSpan GetEndTime()
        {
            return TimeSpan.Parse(EndTime);
        }
    }
}
