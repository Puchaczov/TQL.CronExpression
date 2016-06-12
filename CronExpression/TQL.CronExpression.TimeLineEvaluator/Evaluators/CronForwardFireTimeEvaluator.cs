using System;
using TQL.CronExpression.TimelineEvaluator.List;
using TQL.CronExpression.Parser.Utils;

namespace TQL.CronExpression.TimelineEvaluator.Evaluators
{
    class CronForwardFireTimeEvaluator : ICronFireTimeEvaluator
    {
        private readonly RoundRobinRangeVaryingList<int> dayOfMonths;
        private readonly RoundRobinRangeVaryingList<int> dayOfWeeks;

        private bool expressionExceedTimeBoundary;
        private readonly VirtuallyJoinedList filteredDayOfMonths;
        private readonly RoundRobinRangeVaryingList<int> hours;
        private readonly RoundRobinRangeVaryingList<int> minutes;
        private readonly RoundRobinRangeVaryingList<int> months;

        private readonly Ref<DateTimeOffset> referenceTime;
        private readonly RoundRobinRangeVaryingList<int> seconds;
        private readonly RoundRobinRangeVaryingList<int> years;

        private DateTimeOffset oldReferenceTime;

        public CronForwardFireTimeEvaluator(
            RoundRobinRangeVaryingList<int> years,
            RoundRobinRangeVaryingList<int> months,
            RoundRobinRangeVaryingList<int> dayOfMonths,
            RoundRobinRangeVaryingList<int> dayOfWeeks,
            RoundRobinRangeVaryingList<int> hours,
            RoundRobinRangeVaryingList<int> minutes,
            RoundRobinRangeVaryingList<int> seconds,
            Ref<DateTimeOffset> referenceTime)
        {
            this.expressionExceedTimeBoundary = false;
            this.years = years;
            this.months = months;
            this.dayOfMonths = dayOfMonths;
            this.dayOfWeeks = dayOfWeeks;
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
            this.referenceTime = referenceTime;
            this.filteredDayOfMonths = new VirtuallyJoinedList(this.dayOfMonths, this.dayOfWeeks);
            months.Overflowed += (sender, args) =>
            {
                if (years.WillOverflow())
                {
                    expressionExceedTimeBoundary = true;
                }
                years.Next();
                var refTime = referenceTime.Value;
                referenceTime.Value = new DateTimeOffset(years.Current, 1, 1, 0, 0, 0, refTime.Offset);
            };
            dayOfMonths.Overflowed += (sender, args) =>
            {
                months.Next();
                var refTime = referenceTime.Value;
                referenceTime.Value = new DateTimeOffset(refTime.Year, months.Current, 1, 0, 0, 0, refTime.Offset);
                LimitMonthRange();
            };
            hours.Overflowed += (sender, args) =>
            {
                filteredDayOfMonths.Next();
                var refTime = referenceTime.Value;
                var newDay = filteredDayOfMonths.Count > 0 ? filteredDayOfMonths.Current : 1;
                referenceTime.Value = new DateTimeOffset(refTime.Year, refTime.Month, newDay, 0, 0, 0, refTime.Offset);
            };
            minutes.Overflowed += (sender, args) =>
            {
                hours.Next();
                var refTime = referenceTime.Value;
                referenceTime.Value = new DateTimeOffset(refTime.Year, refTime.Month, refTime.Day, hours.Current, 0, 0, refTime.Offset);
            };
            seconds.Overflowed += (sender, args) =>
            {
                minutes.Next();
                var refTime = referenceTime.Value;
                referenceTime.Value = new DateTimeOffset(refTime.Year, refTime.Month, refTime.Day, refTime.Hour, minutes.Current, 0, refTime.Offset);
            };
            this.Reset();
        }

        public bool IsExceededTimeBoundary => this.expressionExceedTimeBoundary;

        public DateTimeOffset ReferenceTime
        {
            set
            {
                Reset();
                referenceTime.Value = value;
            }
        }

        public bool IsSatisfiedBy(DateTimeOffset time)
        {
            var timeWithoutMillis = new DateTimeOffset(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, time.Offset);
            ReferenceTime = time.AddSeconds(-1);
            DateTimeOffset? score = null;
            Reset();
            do
            {
                score = NextFire();
            }
            while (score.HasValue && referenceTime.Value < timeWithoutMillis);
            return score.HasValue && referenceTime.Value == timeWithoutMillis;
        }

        public void LimitMonthRange()
        {
            var val = DateTime.DaysInMonth(referenceTime.Value.Year, referenceTime.Value.Month) - 1;
            filteredDayOfMonths.SetRange(0, val);
            dayOfWeeks.SetRange(0, val);
            filteredDayOfMonths.RebuildCorrespondingKeys();
        }


        public DateTimeOffset? NextFire()
        {
            referenceTime.Value = referenceTime.Value.AddSeconds(1);
            LimitMonthRange();
            while (true)
            {
                var referenceTime = this.referenceTime.Value;
                while (!years.WillOverflow() && years.Current < referenceTime.Year)
                {
                    years.Next();
                }

                if (IsExceededTimeBoundary)
                {
                    break;
                }

                var sameOrFurtherYear = years.Current >= referenceTime.Year;
                var isChangedMonth = false;
                while (!months.WillOverflow() && months.Current < referenceTime.Month && sameOrFurtherYear)
                {
                    months.Next();
                    isChangedMonth = true;
                }
                if (IsDatePartBefore(years.Current, months.Current))
                {
                    months.Overflow();
                    isChangedMonth = true;
                    continue;
                }
                if (months.Current > referenceTime.Month)
                {
                    referenceTime = new DateTimeOffset(referenceTime.Year, months.Current, 1, 0, 0, 0, referenceTime.Offset);
                    isChangedMonth = true;
                    this.referenceTime.Value = referenceTime;
                }

                if(isChangedMonth)
                {
                    LimitMonthRange();
                }

                var sameOrFurtherMonth = months.Current >= referenceTime.Month;
                var days = filteredDayOfMonths;
                while (!days.WillOverflow() && days.Current < referenceTime.Day && sameOrFurtherMonth)
                {
                    days.Next();
                }
                if (days.Count == 0 || (!sameOrFurtherMonth && IsDatePartBefore(years.Current, months.Current, days.Current)))
                {
                    days.Overflow();
                    continue;
                }
                if (days.Current > referenceTime.Day)
                {
                    referenceTime = new DateTimeOffset(referenceTime.Year, referenceTime.Month, days.Current, 0, 0, 0, referenceTime.Offset);
                    this.referenceTime.Value = referenceTime;
                }

                var sameOrFurtherDayOfMonth = days.Current >= referenceTime.Day;
                while (!hours.WillOverflow() && hours.Current < referenceTime.Hour && sameOrFurtherDayOfMonth)
                {
                    hours.Next();
                }
                if (IsDatePartBefore(years.Current, months.Current, days.Current, hours.Current))
                {
                    hours.Overflow();
                    continue;
                }
                if (hours.Current > referenceTime.Hour)
                {
                    referenceTime = new DateTimeOffset(referenceTime.Year, referenceTime.Month, referenceTime.Day, hours.Current, 0, 0, referenceTime.Offset);
                    this.referenceTime.Value = referenceTime;
                }

                var sameOrFurtherHour = hours.Current >= referenceTime.Hour;
                while (!minutes.WillOverflow() && minutes.Current < referenceTime.Minute && sameOrFurtherHour)
                {
                    minutes.Next();
                }
                if (IsDatePartBefore(years.Current, months.Current, days.Current, hours.Current, minutes.Current))
                {
                    minutes.Overflow();
                    continue;
                }
                if (minutes.Current > referenceTime.Minute)
                {
                    referenceTime = new DateTimeOffset(referenceTime.Year, referenceTime.Month, referenceTime.Day, referenceTime.Hour, minutes.Current, 0, referenceTime.Offset);
                    this.referenceTime.Value = referenceTime;
                }

                var sameOrFurtherMinute = minutes.Current >= referenceTime.Minute;
                while (!seconds.WillOverflow() && seconds.Current < referenceTime.Second && sameOrFurtherMinute)
                {
                    seconds.Next();
                }
                if (IsDatePartBefore(years.Current, months.Current, days.Current, hours.Current, minutes.Current, seconds.Current))
                {
                    seconds.Overflow();
                    continue;
                }
                if (seconds.Current > referenceTime.Second)
                {
                    referenceTime = new DateTimeOffset(referenceTime.Year, referenceTime.Month, referenceTime.Day, referenceTime.Hour, referenceTime.Minute, seconds.Current, referenceTime.Offset);
                    this.referenceTime.Value = referenceTime;
                }
                
                if (IsDatePartBefore(years.Current, months.Current, days.Current, hours.Current, minutes.Current, seconds.Current))
                {
                    seconds.Overflow();
                    continue;
                }

                referenceTime = new DateTimeOffset(
                    years.Current,
                    months.Current,
                    days.Current,
                    hours.Current,
                    minutes.Current,
                    seconds.Current,
                    referenceTime.Offset
                );

                this.referenceTime.Value = referenceTime;

                //There is possibility to generate two times the same date for example 29,L which in
                //leap year, in february. When it happens, just evaluate next date
                if (referenceTime == oldReferenceTime)
                {
                    this.referenceTime.Value = referenceTime.AddSeconds(1);
                    continue;
                }

                oldReferenceTime = referenceTime;

                return referenceTime;
            }

            //Time boundary had beed exceed.
            return null;
        }

        public DateTimeOffset? PreviousFire()
        {
            throw new NotImplementedException();
        }

        private bool IsDatePartBefore(int year1, int month1)
        {
            var referenceTime = this.referenceTime.Value;
            return
                new DateTimeOffset(year1, month1, 1, 0, 0, 0, referenceTime.Offset) <
                new DateTimeOffset(referenceTime.Year, referenceTime.Month, 1, 0, 0, 0, referenceTime.Offset);
        }

        private bool IsDatePartBefore(int year1, int month1, int day1)
        {
            var referenceTime = this.referenceTime.Value;
            return
                new DateTimeOffset(year1, month1, day1, 0, 0, 0, referenceTime.Offset) <
                new DateTimeOffset(referenceTime.Year, referenceTime.Month, referenceTime.Day, 0, 0, 0, referenceTime.Offset);
        }

        private bool IsDatePartBefore(int year1, int month1, int day1, int hours1)
        {
            var referenceTime = this.referenceTime.Value;
            return
                new DateTimeOffset(year1, month1, day1, hours1, 0, 0, 0, referenceTime.Offset) <
                new DateTimeOffset(referenceTime.Year, referenceTime.Month, referenceTime.Day, referenceTime.Hour, 0, 0, referenceTime.Offset);
        }

        private bool IsDatePartBefore(int year1, int month1, int day1, int hours1, int minute1)
        {
            var referenceTime = this.referenceTime.Value;
            return
                new DateTimeOffset(year1, month1, day1, hours1, minute1, 0, referenceTime.Offset) <
                new DateTimeOffset(referenceTime.Year, referenceTime.Month, referenceTime.Day, referenceTime.Hour, referenceTime.Minute, 0, referenceTime.Offset);
        }

        private bool IsDatePartBefore(int year1, int month1, int day1, int hours1, int minute1, int second1)
        {
            var referenceTime = this.referenceTime.Value;
            return
                new DateTimeOffset(year1, month1, day1, hours1, minute1, second1, referenceTime.Offset) <
                new DateTimeOffset(referenceTime.Year, referenceTime.Month, referenceTime.Day, referenceTime.Hour, referenceTime.Minute, referenceTime.Second, referenceTime.Offset);
        }

        private void Reset()
        {
            this.expressionExceedTimeBoundary = false;
            this.years.Reset();
            this.months.Reset();
            this.dayOfMonths.Reset();
            this.dayOfWeeks.Reset();
            this.hours.Reset();
            this.minutes.Reset();
            this.seconds.Reset();
            this.oldReferenceTime = new DateTimeOffset();
            this.filteredDayOfMonths.Reset();
        }
    }
}
