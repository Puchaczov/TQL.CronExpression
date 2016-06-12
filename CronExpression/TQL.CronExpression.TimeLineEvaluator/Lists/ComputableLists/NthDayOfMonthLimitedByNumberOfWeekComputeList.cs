using System;
using TQL.CronExpression.Parser.Utils;

namespace TQL.CronExpression.TimelineEvaluator.Lists.ComputableLists
{
    public class NthDayOfMonthLimitedByNumberOfWeekComputeList : NthDayOfMonthComputeList
    {
        private readonly int numberOfWeek;

        public NthDayOfMonthLimitedByNumberOfWeekComputeList(Ref<DateTimeOffset> referenceTime, DayOfWeek dayOfWeekToFind, int numberOfWeek)
            : base(referenceTime, dayOfWeekToFind)
        {
            this.numberOfWeek = numberOfWeek;
        }

        public override int Element(int index)
        {
            var tmpNumberOfWeek = numberOfWeek - 1;
            if (tmpNumberOfWeek < 0 || tmpNumberOfWeek >= base.Count)
            {
                throw new IndexOutOfRangeException(nameof(tmpNumberOfWeek));
            }
            return base.Element(tmpNumberOfWeek);
        }

        public override int Count => 1;
    }
}
