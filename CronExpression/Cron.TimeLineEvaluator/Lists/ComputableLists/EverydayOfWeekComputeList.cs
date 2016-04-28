using Cron.Parser.Utils;
using System;
using System.Collections.Generic;

namespace Cron.Extensions.TimelineEvaluator.Lists.ComputableLists
{
    public class EverydayOfWeekComputeList : DateTimeBasedComputeList
    {
        public EverydayOfWeekComputeList(Ref<DateTimeOffset> referenceTime)
            : base(referenceTime, new List<int>())
        { }

        public override int Count => DateTime.DaysInMonth(referenceTime.Value.Year, referenceTime.Value.Month);

        public override int Element(int index) => index + 1;
    }
}
