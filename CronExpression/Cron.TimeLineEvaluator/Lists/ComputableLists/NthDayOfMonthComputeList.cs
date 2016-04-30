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
                var daysInMonth = DateTime.DaysInMonth(foundedDate.Year, foundedDate.Month);
                var day = foundedDate.Day;
                if(day + 28 <= daysInMonth)
                {
                    return 5;
                }
                else if(day + 21 <= daysInMonth)
                {
                    return 4;
                }
                else if(day + 14 <= daysInMonth)
                {
                    return 3;
                }
                else if(day + 7 <= daysInMonth)
                {
                    return 2;
                }
                return 1;
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
                    return foundedDate.Day + 7;
                case 2:
                    return foundedDate.Day + 14;
                case 3:
                    return foundedDate.Day + 21;
                case 4:
                    return foundedDate.Day + 28;
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
            var diff = refTime.DayOfWeek - dayOfWeekToFind;
            if(diff > 0)
            {
                return refTime.AddDays((int)((DayOfWeek.Saturday - refTime.DayOfWeek) + 1 + dayOfWeekToFind));
            }
            else if(diff < 0)
            {
                return refTime.AddDays(diff * -1);
            }
            return refTime;
        }
    }
}
