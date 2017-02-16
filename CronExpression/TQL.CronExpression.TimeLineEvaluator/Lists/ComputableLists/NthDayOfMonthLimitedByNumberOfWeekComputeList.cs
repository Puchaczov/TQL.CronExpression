using System;
using TQL.CronExpression.Parser.Utils;

namespace TQL.CronExpression.TimelineEvaluator.Lists.ComputableLists
{
    public class NthDayOfMonthLimitedByNumberOfWeekComputeList : NthDayOfMonthComputeList
    {
        private readonly int _numberOfWeek;

        public NthDayOfMonthLimitedByNumberOfWeekComputeList(Ref<DateTimeOffset> referenceTime,
            DayOfWeek dayOfWeekToFind, int numberOfWeek)
            : base(referenceTime, dayOfWeekToFind)
        {
            this._numberOfWeek = numberOfWeek;
        }

        public override int Count => 1;

        public override int Element(int index)
        {
            var tmpNumberOfWeek = _numberOfWeek - 1;
            if (tmpNumberOfWeek < 0 || tmpNumberOfWeek >= base.Count)
                throw new IndexOutOfRangeException(nameof(tmpNumberOfWeek));
            return base.Element(tmpNumberOfWeek);
        }
    }
}