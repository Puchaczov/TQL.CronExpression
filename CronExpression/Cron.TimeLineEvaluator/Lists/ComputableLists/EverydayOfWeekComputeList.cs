using Cron.Parser.Utils;
using System;
using System.Collections.Generic;

namespace Cron.Extensions.TimelineEvaluator.Lists.ComputableLists
{
    public class EverydayOfWeekComputeList : DateTimeBasedComputeList
    {
        private int daysInMonth;
        private int lastYear;
        private int lastMonth;

        public EverydayOfWeekComputeList(Ref<DateTimeOffset> referenceTime)
            : base(referenceTime, new List<int>())
        {
            lastYear = referenceTime.Value.Year;
            lastMonth = referenceTime.Value.Month;
            daysInMonth = DateTime.DaysInMonth(referenceTime.Value.Year, referenceTime.Value.Month);
        }

        public override int Count
        {
            get
            {
                if(lastYear != referenceTime.Value.Year || lastMonth != referenceTime.Value.Month)
                {
                    lastYear = referenceTime.Value.Year;
                    lastMonth = referenceTime.Value.Month;
                    daysInMonth = DateTime.DaysInMonth(referenceTime.Value.Year, referenceTime.Value.Month);
                    return daysInMonth;
                }
                return daysInMonth;
            }
        }

        public override int Element(int index) => index + 1;
    }
}
