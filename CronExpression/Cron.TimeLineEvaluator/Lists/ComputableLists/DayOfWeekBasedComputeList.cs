using Cron.Parser.Utils;
using System;
using System.Collections.Generic;

namespace Cron.Extensions.TimelineEvaluator.Lists.ComputableLists
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
