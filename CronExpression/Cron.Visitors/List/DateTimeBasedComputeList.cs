using Cron.Parser.Extensions;
using Cron.Parser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Cron.Parser.List
{
    public abstract class DateTimeBasedComputeList : IVirtualList<int>
    {
        protected Ref<DateTimeOffset> referenceTime;
        protected IList<int> list;

        public DateTimeBasedComputeList(Ref<DateTimeOffset> referenceTime, IList<int> list)
        {
            this.referenceTime = referenceTime;
            this.list = list;
        }

        public virtual int Count
        {
            get
            {
                return list.Count;
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
                throw new NotImplementedException();
            }
        }

        public void Add(IVirtualList<int> list)
        {
            throw new NotImplementedException();
        }

        public abstract int Element(int index);

        public IEnumerator<int> GetEnumerator()
        {
            return new VirtualListEnumerator<int>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class MonthBasedComputedList : DateTimeBasedComputeList
    {
        public MonthBasedComputedList(Ref<DateTimeOffset> referenceTime, IList<int> list)
            : base(referenceTime, list)
        { }

        public override int Element(int index)
        {
            var elem = list[index];
            return DateTime.DaysInMonth(referenceTime.Value.Year, referenceTime.Value.Month) - elem;
        }
    }

    public class LastDayOfWeekInMonthBasedOnCurrentMonthComputedList : DateTimeBasedComputeList
    {
        private const int daysInWeek = 7;

        public LastDayOfWeekInMonthBasedOnCurrentMonthComputedList(Ref<DateTimeOffset> referenceTime, IList<int> list)
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

    public class DayOfWeekBasedComputedList : DateTimeBasedComputeList
    {
        public DayOfWeekBasedComputedList(Ref<DateTimeOffset> referenceTime, IList<int> list)
            : base(referenceTime, list)
        { }

        public override int Element(int index)
        {
            var elem = list[index];
            return 7 - elem;
        }
    }

    public class EveryDayOfWeekAllowedList : DateTimeBasedComputeList
    {
        public EveryDayOfWeekAllowedList(Ref<DateTimeOffset> referenceTime)
            : base(referenceTime, new List<int>())
        { }

        public override int Element(int index)
        {
            return index + 1;
        }

        public override int Count
        {
            get
            {
                return DateTime.DaysInMonth(referenceTime.Value.Year, referenceTime.Value.Month);
            }
        }
    }

    /// <summary>
    /// Fully virtual list.
    /// </summary>
    public class NthDayOfMonthList : IVirtualList<int>
    {
        private Ref<DateTimeOffset> referenceTime;
        private DayOfWeek dayOfWeekToFind;

        public NthDayOfMonthList(Ref<DateTimeOffset> referenceTime, DayOfWeek dayOfWeekToFind)
        {
            this.referenceTime = referenceTime;
            this.dayOfWeekToFind = dayOfWeekToFind;
        }

        public virtual int Element(int index)
        {
            var foundedDate = GetFirstMatchingDateInMonth();
            if(index == 0)
            {
                return foundedDate.Day;
            }
            else if(index == 1)
            {
                return foundedDate.AddDays(7).Day;
            }
            else if(index == 2)
            {
                return foundedDate.AddDays(14).Day;
            }
            else if(index == 3)
            {
                return foundedDate.AddDays(21).Day;
            }
            else if(index == 4)
            {
                return foundedDate.AddDays(28).Day;
            }
            else
            {
                throw new IndexOutOfRangeException(nameof(index));
            }
        }

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

        public void Add(IVirtualList<int> list)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new VirtualListEnumerator<int>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual int Count
        {
            get
            {
                var foundedDate = GetFirstMatchingDateInMonth();
                var month = foundedDate.Month;
                int count = 1;
                while((foundedDate = foundedDate.AddDays(7)).Month == month)
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
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }

    public class NthDayOfMonthLimitedByNumberOfWeekList : NthDayOfMonthList
    {
        private int numberOfWeek;

        public NthDayOfMonthLimitedByNumberOfWeekList(Ref<DateTimeOffset> referenceTime, DayOfWeek dayOfWeekToFind, int numberOfWeek)
            : base(referenceTime, dayOfWeekToFind)
        {
            this.numberOfWeek = numberOfWeek;
        }

        public override int Element(int index)
        {
            var tmpNumberOfWeek = numberOfWeek - 1;
            if(tmpNumberOfWeek < 0 || tmpNumberOfWeek >= base.Count)
            {
                throw new IndexOutOfRangeException(nameof(tmpNumberOfWeek));
            }
            return base.Element(tmpNumberOfWeek);
        }

        public override int Count
        {
            get
            {
                return 1;
            }
        }
    }
}
