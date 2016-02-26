using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Enums
{
    public enum Segment : short
    {
        Seconds,
        Minutes,
        Hours,
        DayOfMonth,
        Month,
        DayOfWeek,
        Year
    }
}
