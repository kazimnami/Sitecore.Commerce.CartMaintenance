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
            StopOverrun = false;
        }

        public int DaysToRetainCarts { get; set; }
        public bool StopOverrun { get; set; }

        public List<Schedule> AllowedSchedules { get; set; }
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
