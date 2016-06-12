using System;
using System.Collections.Generic;
using TQL.CronExpression.Parser.Utils;

namespace TQL.CronExpression.TimelineEvaluator.Lists.ComputableLists
{
    public class DayOfWeekBasedComputeList : DateTimeBasedComputeList
    {
        public DayOfWeekBasedComputeList(Ref<DateTimeOffset> referenceTime, IList<int> list)
            : base(referenceTime, list)
        { }

        public override int Element(int index)
        {
            var elem = list[index];
            return 7 - elem;
        }
    }
}
