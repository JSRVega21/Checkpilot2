using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPilot.Models
{
    public enum SyncStatus
    {
        New = 1,
        Updated = 2,
        Deleted = 3,
        Synced = 4,
        None = 99
    }

    public enum DaysOfWeek
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }

    public enum MonthNames
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
}

