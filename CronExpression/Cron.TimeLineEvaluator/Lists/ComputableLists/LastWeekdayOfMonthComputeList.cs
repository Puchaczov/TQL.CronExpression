using Cron.Parser.Utils;
using System;

namespace Cron.Extensions.TimelineEvaluator.Lists.ComputableLists
{
    public class LastWeekdayOfMonthComputeList : DateTimeBasedComputeList
    {
        public LastWeekdayOfMonthComputeList(Ref<DateTimeOffset> referenceTime)
            : base(referenceTime, null)
        { }

        public override int Element(int index)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            var refTime = referenceTime.Value;
            var daysInMonth = DateTime.DaysInMonth(refTime.Year, refTime.Month);
            var day = new DateTime(refTime.Year, refTime.Month, daysInMonth);
            if (day.DayOfWeek == DayOfWeek.Saturday)
            {
                return daysInMonth - 1;
            }
            else if (day.DayOfWeek == DayOfWeek.Sunday)
            {
                return daysInMonth - 2;
            }
            return daysInMonth;
        }

        public override int Count => 1;
    }
}
