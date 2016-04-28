using Cron.Extensions.TimelineEvaluator.List;
using Cron.Parser.Utils;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cron.Extensions.TimelineEvaluator.Lists.ComputableLists
{
    /// <summary>
    /// Fully virtual list.
    /// </summary>
    public class NthDayOfMonthComputeList : IComputableElementsList<int>
    {
        private readonly DayOfWeek dayOfWeekToFind;
        private readonly Ref<DateTimeOffset> referenceTime;

        public NthDayOfMonthComputeList(Ref<DateTimeOffset> referenceTime, DayOfWeek dayOfWeekToFind)
        {
            this.referenceTime = referenceTime;
            this.dayOfWeekToFind = dayOfWeekToFind;
        }

        public virtual int Count
        {
            get
            {
                var foundedDate = GetFirstMatchingDateInMonth();
                var month = foundedDate.Month;
                var count = 1;
                while ((foundedDate = foundedDate.AddDays(7)).Month == month)
                {
                    count += 1;
                }
                return count;
            }
        }

        public int this[int index]
        {
            get
            {
                return Element(index);
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        public void Add(IComputableElementsList<int> list)
        {
            throw new NotImplementedException();
        }

        public virtual int Element(int index)
        {
            var foundedDate = GetFirstMatchingDateInMonth();
            switch (index)
            {
                case 0:
                    return foundedDate.Day;
                case 1:
                    return foundedDate.AddDays(7).Day;
                case 2:
                    return foundedDate.AddDays(14).Day;
                case 3:
                    return foundedDate.AddDays(21).Day;
                case 4:
                    return foundedDate.AddDays(28).Day;
                default:
                    throw new IndexOutOfRangeException(nameof(index));
            }
        }

        public IEnumerator<int> GetEnumerator() => new ComputableElementsEnumerator<int>(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private DateTimeOffset GetFirstMatchingDateInMonth()
        {
            var val = referenceTime.Value;
            var refTime = new DateTimeOffset(val.Year, val.Month, 1, 0, 0, 0, new TimeSpan(val.Offset.Days, val.Offset.Hours, val.Offset.Minutes, val.Offset.Seconds));
            while (refTime.DayOfWeek != dayOfWeekToFind)
            {
                refTime = refTime.AddDays(1);
            }
            return refTime;
        }
    }
}
