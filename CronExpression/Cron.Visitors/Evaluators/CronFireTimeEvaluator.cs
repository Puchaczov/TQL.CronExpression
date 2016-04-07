using Cron.Parser.Extensions;
using Cron.Parser.List;
using Cron.Parser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Visitors.Evaluators
{
    class CronFireTimeEvaluator : ICronFireTimeEvaluator
    {
        private RoundRobinRangeVaryingList<int> years;
        private RoundRobinRangeVaryingList<int> months;
        private RoundRobinRangeVaryingList<int> dayOfMonths;
        private RoundRobinRangeVaryingList<int> dayOfWeeks;
        private RoundRobinRangeVaryingList<int> hours;
        private RoundRobinRangeVaryingList<int> minutes;
        private RoundRobinRangeVaryingList<int> seconds;
        private VirtuallyJoinedList filteredDayOfMonths;

        private Ref<DateTimeOffset> referenceTime;

        public DateTime ReferenceTime
        {
            set
            {
                referenceTime.Value = value;
            }
        }

        public DateTimeOffset OffsetReferenceTime
        {
            set
            {
                referenceTime.Value = value;
            }
        }

        public CronFireTimeEvaluator(
            RoundRobinRangeVaryingList<int> years,
            RoundRobinRangeVaryingList<int> months,
            RoundRobinRangeVaryingList<int> dayOfMonths,
            RoundRobinRangeVaryingList<int> dayOfWeeks,
            RoundRobinRangeVaryingList<int> hours,
            RoundRobinRangeVaryingList<int> minutes,
            RoundRobinRangeVaryingList<int> seconds,
            Ref<DateTimeOffset> referenceTime)
        {
            this.years = years;
            this.months = months;
            this.dayOfMonths = dayOfMonths;
            this.dayOfWeeks = dayOfWeeks;
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
            this.referenceTime = referenceTime;
            this.filteredDayOfMonths = new VirtuallyJoinedList(this.dayOfMonths, this.dayOfWeeks);
            months.Overflowed += (sender, args) => {
                years.Next();
                var refTime = referenceTime.Value;
                referenceTime.Value = new DateTimeOffset(years.Current, 1, 1, 0, 0, 0, refTime.Offset);
            };
            dayOfMonths.Overflowed += (sender, args) => {
                months.Next();
                var refTime = referenceTime.Value;
                referenceTime.Value = new DateTimeOffset(refTime.Year, months.Current, 1, 0, 0, 0, refTime.Offset);
                LimitMonthRange();
            };
            hours.Overflowed += (sender, args) => {
                filteredDayOfMonths.Next();
                var refTime = referenceTime.Value;
                var newDay = filteredDayOfMonths.Count > 0 ? filteredDayOfMonths.Current : 1;
                referenceTime.Value = new DateTimeOffset(refTime.Year, refTime.Month, newDay, 0, 0, 0, refTime.Offset);
            };
            minutes.Overflowed += (sender, args) => {
                hours.Next();
                var refTime = referenceTime.Value;
                referenceTime.Value = new DateTimeOffset(refTime.Year, refTime.Month, refTime.Day, hours.Current, 0, 0, refTime.Offset);
            };
            seconds.Overflowed += (sender, args) => {
                minutes.Next();
                var refTime = referenceTime.Value;
                referenceTime.Value = new DateTimeOffset(refTime.Year, refTime.Month, refTime.Day, refTime.Hour, minutes.Current, 0, refTime.Offset);
            };
        }


        public DateTime NextFire()
        {
            referenceTime.Value = referenceTime.Value.AddSeconds(1);
            LimitMonthRange();
            filteredDayOfMonths.RebuildCorrespondingKeys();
            while (true)
            {
                var referenceTime = this.referenceTime.Value;
                while (!years.WillOverflow() && years.Current < referenceTime.Year)
                {
                    years.Next();
                }

                var sameOrFurtherYear = years.Current >= referenceTime.Year;
                while (!months.WillOverflow() && months.Current < referenceTime.Month && sameOrFurtherYear)
                {
                    months.Next();
                }
                if (!sameOrFurtherYear && IsDatePartBefore(years.Current, months.Current))
                {
                    months.Overflow();
                    continue;
                }
                if (months.Current > referenceTime.Month)
                {
                    referenceTime = new DateTimeOffset(referenceTime.Year, months.Current, 1, 0, 0, 0, referenceTime.Offset);
                    this.referenceTime.Value = referenceTime;
                }

                LimitMonthRange();

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
                if(days.Current > referenceTime.Day)
                {
                    referenceTime = new DateTimeOffset(referenceTime.Year, referenceTime.Month, days.Current, 0, 0, 0, referenceTime.Offset);
                    this.referenceTime.Value = referenceTime;
                }

                var sameOrFurtherDayOfMonth = days.Current >= referenceTime.Day;
                while (!hours.WillOverflow() && hours.Current < referenceTime.Hour && sameOrFurtherDayOfMonth)
                {
                    hours.Next();
                }
                if (!sameOrFurtherDayOfMonth && IsDatePartBefore(years.Current, months.Current, days.Current, hours.Current))
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
                if (!sameOrFurtherHour && IsDatePartBefore(years.Current, months.Current, days.Current, hours.Current, minutes.Current))
                {
                    minutes.Overflow();
                    continue;
                }
                if(minutes.Current > referenceTime.Minute)
                {
                    referenceTime = new DateTimeOffset(referenceTime.Year, referenceTime.Month, referenceTime.Day, referenceTime.Hour, minutes.Current, 0, referenceTime.Offset);
                    this.referenceTime.Value = referenceTime;
                }

                var sameOrFurtherMinute = minutes.Current >= referenceTime.Minute;
                while (!seconds.WillOverflow() && seconds.Current < referenceTime.Second && sameOrFurtherMinute)
                {
                    seconds.Next();
                }
                if (!sameOrFurtherMinute && IsDatePartBefore(years.Current, months.Current, days.Current, hours.Current, minutes.Current, seconds.Current))
                {
                    seconds.Overflow();
                    continue;
                }
                if (seconds.Current > referenceTime.Second)
                {
                    referenceTime = new DateTimeOffset(referenceTime.Year, referenceTime.Month, referenceTime.Day, referenceTime.Hour, referenceTime.Minute, seconds.Current, referenceTime.Offset);
                    this.referenceTime.Value = referenceTime;
                }

                var sameOrFurtherSecond = seconds.Current >= referenceTime.Second;
                if (!sameOrFurtherSecond && IsDatePartBefore(years.Current, months.Current, days.Current, hours.Current, minutes.Current, seconds.Current))
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

                return referenceTime.DateTime;
            }
        }

        public DateTime PreviousFire()
        {
            throw new NotImplementedException();
        }

        public void LimitMonthRange()
        {
            var val = DateTime.DaysInMonth(referenceTime.Value.Year, referenceTime.Value.Month) - 1;
            filteredDayOfMonths.SetRange(0, val);
            dayOfWeeks.SetRange(0, val);
            filteredDayOfMonths.RebuildCorrespondingKeys();
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
    }
}
