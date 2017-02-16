using System;
using System.Collections.Generic;
using TQL.CronExpression.Parser.Utils;

namespace TQL.CronExpression.TimelineEvaluator.Lists.ComputableLists
{
    public class MonthBasedComputeList : DateTimeBasedComputeList
    {
        public MonthBasedComputeList(Ref<DateTimeOffset> referenceTime, IList<int> list)
            : base(referenceTime, list)
        {
        }

        public override int Element(int index)
        {
            var elem = List[index];
            return DateTime.DaysInMonth(ReferenceTime.Value.Year, ReferenceTime.Value.Month) - elem;
        }
    }
}