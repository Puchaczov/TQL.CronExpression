using System;
using System.Collections.Generic;
using TQL.CronExpression.Parser.Utils;

namespace TQL.CronExpression.Extensions.TimelineEvaluator.Lists.ComputableLists
{
    public class MonthBasedComputeList : DateTimeBasedComputeList
    {
        public MonthBasedComputeList(Ref<DateTimeOffset> referenceTime, IList<int> list)
            : base(referenceTime, list)
        { }

        public override int Element(int index)
        {
            var elem = list[index];
            return DateTime.DaysInMonth(referenceTime.Value.Year, referenceTime.Value.Month) - elem;
        }
    }
}
