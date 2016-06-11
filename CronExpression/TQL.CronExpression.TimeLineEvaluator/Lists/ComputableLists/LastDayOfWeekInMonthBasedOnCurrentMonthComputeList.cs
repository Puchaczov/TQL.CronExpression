using System;
using System.Collections.Generic;
using TQL.CronExpression.Parser.Extensions;
using TQL.CronExpression.Parser.Utils;

namespace TQL.CronExpression.Extensions.TimelineEvaluator.Lists.ComputableLists
{
    public class LastDayOfWeekInMonthBasedOnCurrentMonthComputeList : DateTimeBasedComputeList
    {
        private const int daysInWeek = 7;

        public LastDayOfWeekInMonthBasedOnCurrentMonthComputeList(Ref<DateTimeOffset> referenceTime, IList<int> list)
            : base(referenceTime, list)
        { }

        /// <summary>
        /// This function compute last days in month based on elements stored in list.
        /// List will contain values from 1 to 7 which means that what needs to be found is: last Friday, last Saturday etc...
        /// Current month is referenced by DateTimeOffset.
        /// How algorithm work:
        /// Based on last day of month, we need its WeekDay (Monday, Tuesday, ..., Saturday)
        /// Firstly, Check difference between LastWeekDay and ExpectedWeekDay. It will measure how far your ExpectedWeekDay is from LastWeekDay.
        /// Secondly, compute days to remove from LastDay. It may occur overflow or underflow which means that it's jump to next/prev week.
        /// Formula:
        /// DaysInWeek - ((ExpectedDayOfWeek - LastDayOfMonth) % DaysInWeek)
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Computed day of month</returns>
        public override int Element(int index)
        {
            var days = DateTime.DaysInMonth(referenceTime.Value.Year, referenceTime.Value.Month);
            var dayOfMonth = new DateTime(referenceTime.Value.Year, referenceTime.Value.Month, days);
            var lastDayOfMonth = dayOfMonth.DayOfWeek.AsInt();
            var expectedDayOfWeek = list[index];
            var weekdayDiff = expectedDayOfWeek - lastDayOfMonth;
            var howMuchFarFromExpectedDay = (daysInWeek - (expectedDayOfWeek - lastDayOfMonth)) % daysInWeek;
            return days - howMuchFarFromExpectedDay;
        }
    }
}
