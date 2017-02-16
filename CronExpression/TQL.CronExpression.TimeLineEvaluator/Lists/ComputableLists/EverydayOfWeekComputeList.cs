using System;
using System.Collections.Generic;
using TQL.CronExpression.Parser.Utils;

namespace TQL.CronExpression.TimelineEvaluator.Lists.ComputableLists
{
    public class EverydayOfWeekComputeList : DateTimeBasedComputeList
    {
        private int _daysInMonth;
        private int _lastMonth;
        private int _lastYear;

        public EverydayOfWeekComputeList(Ref<DateTimeOffset> referenceTime)
            : base(referenceTime, new List<int>())
        {
            _lastYear = referenceTime.Value.Year;
            _lastMonth = referenceTime.Value.Month;
            _daysInMonth = DateTime.DaysInMonth(referenceTime.Value.Year, referenceTime.Value.Month);
        }

        public override int Count
        {
            get
            {
                if (_lastYear != ReferenceTime.Value.Year || _lastMonth != ReferenceTime.Value.Month)
                {
                    _lastYear = ReferenceTime.Value.Year;
                    _lastMonth = ReferenceTime.Value.Month;
                    _daysInMonth = DateTime.DaysInMonth(ReferenceTime.Value.Year, ReferenceTime.Value.Month);
                    return _daysInMonth;
                }
                return _daysInMonth;
            }
        }

        public override int Element(int index) => index + 1;
    }
}