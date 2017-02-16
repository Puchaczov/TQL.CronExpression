using System;
using TQL.CronExpression.Parser.Utils;
using TQL.CronExpression.TimelineEvaluator.Lists;

namespace TQL.CronExpression.TimelineEvaluator.Evaluators
{
    class CronForwardFireTimeEvaluator : ICronFireTimeEvaluator
    {
        private readonly RoundRobinRangeVaryingList<int> _dayOfMonths;
        private readonly RoundRobinRangeVaryingList<int> _dayOfWeeks;
        private readonly VirtuallyJoinedList _filteredDayOfMonths;
        private readonly RoundRobinRangeVaryingList<int> _hours;
        private readonly RoundRobinRangeVaryingList<int> _minutes;
        private readonly RoundRobinRangeVaryingList<int> _months;

        private readonly Ref<DateTimeOffset> _referenceTime;
        private readonly RoundRobinRangeVaryingList<int> _seconds;
        private readonly RoundRobinRangeVaryingList<int> _years;

        private DateTimeOffset _oldReferenceTime;

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
            IsExceededTimeBoundary = false;
            this._years = years;
            this._months = months;
            this._dayOfMonths = dayOfMonths;
            this._dayOfWeeks = dayOfWeeks;
            this._hours = hours;
            this._minutes = minutes;
            this._seconds = seconds;
            this._referenceTime = referenceTime;
            _filteredDayOfMonths = new VirtuallyJoinedList(this._dayOfMonths, this._dayOfWeeks);
            months.Overflowed += (sender, args) =>
            {
                if (years.WillOverflow())
                    IsExceededTimeBoundary = true;
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
                _filteredDayOfMonths.Next();
                var refTime = referenceTime.Value;
                var newDay = _filteredDayOfMonths.Count > 0 ? _filteredDayOfMonths.Current : 1;
                referenceTime.Value = new DateTimeOffset(refTime.Year, refTime.Month, newDay, 0, 0, 0, refTime.Offset);
            };
            minutes.Overflowed += (sender, args) =>
            {
                hours.Next();
                var refTime = referenceTime.Value;
                referenceTime.Value = new DateTimeOffset(refTime.Year, refTime.Month, refTime.Day, hours.Current, 0, 0,
                    refTime.Offset);
            };
            seconds.Overflowed += (sender, args) =>
            {
                minutes.Next();
                var refTime = referenceTime.Value;
                referenceTime.Value = new DateTimeOffset(refTime.Year, refTime.Month, refTime.Day, refTime.Hour,
                    minutes.Current, 0, refTime.Offset);
            };
            Reset();
        }

        public bool IsExceededTimeBoundary { get; private set; }

        public DateTimeOffset ReferenceTime
        {
            set
            {
                Reset();
                _referenceTime.Value = value;
            }
        }

        public bool IsSatisfiedBy(DateTimeOffset time)
        {
            var timeWithoutMillis = new DateTimeOffset(time.Year, time.Month, time.Day, time.Hour, time.Minute,
                time.Second, time.Offset);
            ReferenceTime = time.AddSeconds(-1);
            DateTimeOffset? score = null;
            Reset();
            do
            {
                score = NextFire();
            } while (score.HasValue && _referenceTime.Value < timeWithoutMillis);
            return score.HasValue && _referenceTime.Value == timeWithoutMillis;
        }


        public DateTimeOffset? NextFire()
        {
            _referenceTime.Value = _referenceTime.Value.AddSeconds(1);
            LimitMonthRange();
            while (true)
            {
                var referenceTime = this._referenceTime.Value;
                while (!_years.WillOverflow() && _years.Current < referenceTime.Year)
                    _years.Next();

                if (IsExceededTimeBoundary)
                    break;

                var sameOrFurtherYear = _years.Current >= referenceTime.Year;
                var isChangedMonth = false;
                while (!_months.WillOverflow() && _months.Current < referenceTime.Month && sameOrFurtherYear)
                {
                    _months.Next();
                    isChangedMonth = true;
                }
                if (IsDatePartBefore(_years.Current, _months.Current))
                {
                    _months.Overflow();
                    isChangedMonth = true;
                    continue;
                }
                if (_months.Current > referenceTime.Month)
                {
                    referenceTime = new DateTimeOffset(referenceTime.Year, _months.Current, 1, 0, 0, 0,
                        referenceTime.Offset);
                    isChangedMonth = true;
                    this._referenceTime.Value = referenceTime;
                }

                if (isChangedMonth)
                    LimitMonthRange();

                var sameOrFurtherMonth = _months.Current >= referenceTime.Month;
                var days = _filteredDayOfMonths;
                while (!days.WillOverflow() && days.Current < referenceTime.Day && sameOrFurtherMonth)
                    days.Next();
                if (days.Count == 0 ||
                    !sameOrFurtherMonth && IsDatePartBefore(_years.Current, _months.Current, days.Current))
                {
                    days.Overflow();
                    continue;
                }
                if (days.Current > referenceTime.Day)
                {
                    referenceTime = new DateTimeOffset(referenceTime.Year, referenceTime.Month, days.Current, 0, 0, 0,
                        referenceTime.Offset);
                    this._referenceTime.Value = referenceTime;
                }

                var sameOrFurtherDayOfMonth = days.Current >= referenceTime.Day;
                while (!_hours.WillOverflow() && _hours.Current < referenceTime.Hour && sameOrFurtherDayOfMonth)
                    _hours.Next();
                if (IsDatePartBefore(_years.Current, _months.Current, days.Current, _hours.Current))
                {
                    _hours.Overflow();
                    continue;
                }
                if (_hours.Current > referenceTime.Hour)
                {
                    referenceTime = new DateTimeOffset(referenceTime.Year, referenceTime.Month, referenceTime.Day,
                        _hours.Current, 0, 0, referenceTime.Offset);
                    this._referenceTime.Value = referenceTime;
                }

                var sameOrFurtherHour = _hours.Current >= referenceTime.Hour;
                while (!_minutes.WillOverflow() && _minutes.Current < referenceTime.Minute && sameOrFurtherHour)
                    _minutes.Next();
                if (IsDatePartBefore(_years.Current, _months.Current, days.Current, _hours.Current, _minutes.Current))
                {
                    _minutes.Overflow();
                    continue;
                }
                if (_minutes.Current > referenceTime.Minute)
                {
                    referenceTime = new DateTimeOffset(referenceTime.Year, referenceTime.Month, referenceTime.Day,
                        referenceTime.Hour, _minutes.Current, 0, referenceTime.Offset);
                    this._referenceTime.Value = referenceTime;
                }

                var sameOrFurtherMinute = _minutes.Current >= referenceTime.Minute;
                while (!_seconds.WillOverflow() && _seconds.Current < referenceTime.Second && sameOrFurtherMinute)
                    _seconds.Next();
                if (IsDatePartBefore(_years.Current, _months.Current, days.Current, _hours.Current, _minutes.Current,
                    _seconds.Current))
                {
                    _seconds.Overflow();
                    continue;
                }
                if (_seconds.Current > referenceTime.Second)
                {
                    referenceTime = new DateTimeOffset(referenceTime.Year, referenceTime.Month, referenceTime.Day,
                        referenceTime.Hour, referenceTime.Minute, _seconds.Current, referenceTime.Offset);
                    this._referenceTime.Value = referenceTime;
                }

                if (IsDatePartBefore(_years.Current, _months.Current, days.Current, _hours.Current, _minutes.Current,
                    _seconds.Current))
                {
                    _seconds.Overflow();
                    continue;
                }

                referenceTime = new DateTimeOffset(
                    _years.Current,
                    _months.Current,
                    days.Current,
                    _hours.Current,
                    _minutes.Current,
                    _seconds.Current,
                    referenceTime.Offset
                );

                this._referenceTime.Value = referenceTime;

                //There is possibility to generate two times the same date for example 29,L which in
                //leap year, in february. When it happens, just evaluate next date
                if (referenceTime == _oldReferenceTime)
                {
                    this._referenceTime.Value = referenceTime.AddSeconds(1);
                    continue;
                }

                _oldReferenceTime = referenceTime;

                return referenceTime;
            }

            //Time boundary had beed exceed.
            return null;
        }

        public void LimitMonthRange()
        {
            var val = DateTime.DaysInMonth(_referenceTime.Value.Year, _referenceTime.Value.Month) - 1;
            _filteredDayOfMonths.SetRange(0, val);
            _dayOfWeeks.SetRange(0, val);
            _filteredDayOfMonths.RebuildCorrespondingKeys();
        }

        public DateTimeOffset? PreviousFire()
        {
            throw new NotImplementedException();
        }

        private bool IsDatePartBefore(int year1, int month1)
        {
            var referenceTime = this._referenceTime.Value;
            return
                new DateTimeOffset(year1, month1, 1, 0, 0, 0, referenceTime.Offset) <
                new DateTimeOffset(referenceTime.Year, referenceTime.Month, 1, 0, 0, 0, referenceTime.Offset);
        }

        private bool IsDatePartBefore(int year1, int month1, int day1)
        {
            var referenceTime = this._referenceTime.Value;
            return
                new DateTimeOffset(year1, month1, day1, 0, 0, 0, referenceTime.Offset) <
                new DateTimeOffset(referenceTime.Year, referenceTime.Month, referenceTime.Day, 0, 0, 0,
                    referenceTime.Offset);
        }

        private bool IsDatePartBefore(int year1, int month1, int day1, int hours1)
        {
            var referenceTime = this._referenceTime.Value;
            return
                new DateTimeOffset(year1, month1, day1, hours1, 0, 0, 0, referenceTime.Offset) <
                new DateTimeOffset(referenceTime.Year, referenceTime.Month, referenceTime.Day, referenceTime.Hour, 0, 0,
                    referenceTime.Offset);
        }

        private bool IsDatePartBefore(int year1, int month1, int day1, int hours1, int minute1)
        {
            var referenceTime = this._referenceTime.Value;
            return
                new DateTimeOffset(year1, month1, day1, hours1, minute1, 0, referenceTime.Offset) <
                new DateTimeOffset(referenceTime.Year, referenceTime.Month, referenceTime.Day, referenceTime.Hour,
                    referenceTime.Minute, 0, referenceTime.Offset);
        }

        private bool IsDatePartBefore(int year1, int month1, int day1, int hours1, int minute1, int second1)
        {
            var referenceTime = this._referenceTime.Value;
            return
                new DateTimeOffset(year1, month1, day1, hours1, minute1, second1, referenceTime.Offset) <
                new DateTimeOffset(referenceTime.Year, referenceTime.Month, referenceTime.Day, referenceTime.Hour,
                    referenceTime.Minute, referenceTime.Second, referenceTime.Offset);
        }

        private void Reset()
        {
            IsExceededTimeBoundary = false;
            _years.Reset();
            _months.Reset();
            _dayOfMonths.Reset();
            _dayOfWeeks.Reset();
            _hours.Reset();
            _minutes.Reset();
            _seconds.Reset();
            _oldReferenceTime = new DateTimeOffset();
            _filteredDayOfMonths.Reset();
        }
    }
}