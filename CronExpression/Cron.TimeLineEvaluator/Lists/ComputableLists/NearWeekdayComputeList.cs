using Cron.Parser.Utils;
using System;
using System.Collections.Generic;

namespace Cron.Extensions.TimelineEvaluator.Lists.ComputableLists
{
    public class NearWeekdayComputeList : DateTimeBasedComputeList
    {
        public NearWeekdayComputeList(Ref<DateTimeOffset> referenceTime, int dayOfMonth)
            : base(referenceTime, new List<int> { dayOfMonth })
        { }

        public override int Element(int index)
        {
            var refTime = referenceTime.Value;
            var candidateTime = new DateTime(refTime.Year, refTime.Month, list[0]);
            if (candidateTime.DayOfWeek != DayOfWeek.Saturday && candidateTime.DayOfWeek != DayOfWeek.Sunday)
            {
                return list[0];
            }
            if (candidateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                var friday = candidateTime.AddDays(-1);
                //still the same month
                return friday.Month == candidateTime.Month ? list[0] - 1 : list[0] + 2;
            }
            else
            {
                var monday = candidateTime.AddDays(1);
                //still the same month
                return monday.Month == candidateTime.Month ? list[0] + 1 : list[0] - 2;
            }
        }

        public override int Count
        {
            get
            {
                if (DateTime.DaysInMonth(referenceTime.Value.Year, referenceTime.Value.Month) < list[0])
                {
                    return 0;
                }
                return 1;
            }
        }
    }
}
