using System;
using System.Collections.Generic;
using TQL.CronExpression.Parser.Utils;

namespace TQL.CronExpression.TimelineEvaluator.Lists.ComputableLists
{
    public class NearWeekdayComputeList : DateTimeBasedComputeList
    {
        public NearWeekdayComputeList(Ref<DateTimeOffset> referenceTime, int dayOfMonth)
            : base(referenceTime, new List<int> {dayOfMonth})
        {
        }

        public override int Count
        {
            get
            {
                if (DateTime.DaysInMonth(ReferenceTime.Value.Year, ReferenceTime.Value.Month) < List[0])
                    return 0;
                return 1;
            }
        }

        public override int Element(int index)
        {
            var refTime = ReferenceTime.Value;
            var candidateTime = new DateTime(refTime.Year, refTime.Month, List[0]);
            if (candidateTime.DayOfWeek != DayOfWeek.Saturday && candidateTime.DayOfWeek != DayOfWeek.Sunday)
                return List[0];
            if (candidateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                var friday = candidateTime.AddDays(-1);
                //still the same month
                return friday.Month == candidateTime.Month ? List[0] - 1 : List[0] + 2;
            }
            var monday = candidateTime.AddDays(1);
            //still the same month
            return monday.Month == candidateTime.Month ? List[0] + 1 : List[0] - 2;
        }
    }
}