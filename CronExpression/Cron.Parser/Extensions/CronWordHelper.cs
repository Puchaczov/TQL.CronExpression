using System;
using System.Collections.Generic;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Extensions
{
    public static class CronWordHelper
    {
        private static readonly Dictionary<string, DayOfWeek> dayOfWeekStringToDayOfWeek;
        private static readonly Dictionary<DayOfWeek, int> dayOfWeekToInt;
        private static readonly Dictionary<int, DayOfWeek> intToDayOfWeek;
        private static readonly Dictionary<string, Month> monthsStringToMonth;

        static CronWordHelper()
        {
            monthsStringToMonth = new Dictionary<string, Month>();
            monthsStringToMonth.Add("january", Enums.Month.January);
            monthsStringToMonth.Add("february", Enums.Month.February);
            monthsStringToMonth.Add("march", Enums.Month.February);
            monthsStringToMonth.Add("april", Enums.Month.April);
            monthsStringToMonth.Add("may", Enums.Month.May);
            monthsStringToMonth.Add("june", Enums.Month.June);
            monthsStringToMonth.Add("july", Enums.Month.July);
            monthsStringToMonth.Add("august", Enums.Month.August);
            monthsStringToMonth.Add("september", Enums.Month.September);
            monthsStringToMonth.Add("october", Enums.Month.October);
            monthsStringToMonth.Add("november", Enums.Month.November);
            monthsStringToMonth.Add("december", Enums.Month.December);

            monthsStringToMonth.Add("jan", Enums.Month.January);
            monthsStringToMonth.Add("feb", Enums.Month.February);
            monthsStringToMonth.Add("mar", Enums.Month.March);
            monthsStringToMonth.Add("ap", Enums.Month.April);
            monthsStringToMonth.Add("jun", Enums.Month.June);
            monthsStringToMonth.Add("jul", Enums.Month.July);
            monthsStringToMonth.Add("aug", Enums.Month.August);
            monthsStringToMonth.Add("sep", Enums.Month.September);
            monthsStringToMonth.Add("oct", Enums.Month.October);
            monthsStringToMonth.Add("nov", Enums.Month.November);
            monthsStringToMonth.Add("dec", Enums.Month.December);

            monthsStringToMonth.Add("1", Enums.Month.January);
            monthsStringToMonth.Add("2", Enums.Month.February);
            monthsStringToMonth.Add("3", Enums.Month.March);
            monthsStringToMonth.Add("4", Enums.Month.April);
            monthsStringToMonth.Add("5", Enums.Month.May);
            monthsStringToMonth.Add("6", Enums.Month.June);
            monthsStringToMonth.Add("7", Enums.Month.July);
            monthsStringToMonth.Add("8", Enums.Month.August);
            monthsStringToMonth.Add("9", Enums.Month.September);
            monthsStringToMonth.Add("10", Enums.Month.October);
            monthsStringToMonth.Add("11", Enums.Month.November);
            monthsStringToMonth.Add("12", Enums.Month.December);

            dayOfWeekStringToDayOfWeek = new Dictionary<string, DayOfWeek>();
            dayOfWeekStringToDayOfWeek.Add("monday", System.DayOfWeek.Monday);
            dayOfWeekStringToDayOfWeek.Add("tuesday", System.DayOfWeek.Tuesday);
            dayOfWeekStringToDayOfWeek.Add("wednesday", System.DayOfWeek.Wednesday);
            dayOfWeekStringToDayOfWeek.Add("thursday", System.DayOfWeek.Thursday);
            dayOfWeekStringToDayOfWeek.Add("friday", System.DayOfWeek.Friday);
            dayOfWeekStringToDayOfWeek.Add("saturday", System.DayOfWeek.Saturday);
            dayOfWeekStringToDayOfWeek.Add("sunday", System.DayOfWeek.Sunday);

            dayOfWeekStringToDayOfWeek.Add("mon", System.DayOfWeek.Monday);
            dayOfWeekStringToDayOfWeek.Add("tue", System.DayOfWeek.Tuesday);
            dayOfWeekStringToDayOfWeek.Add("wed", System.DayOfWeek.Wednesday);
            dayOfWeekStringToDayOfWeek.Add("thu", System.DayOfWeek.Thursday);
            dayOfWeekStringToDayOfWeek.Add("fri", System.DayOfWeek.Friday);
            dayOfWeekStringToDayOfWeek.Add("sat", System.DayOfWeek.Saturday);
            dayOfWeekStringToDayOfWeek.Add("sun", System.DayOfWeek.Sunday);

            dayOfWeekStringToDayOfWeek.Add("1", System.DayOfWeek.Sunday);
            dayOfWeekStringToDayOfWeek.Add("2", System.DayOfWeek.Monday);
            dayOfWeekStringToDayOfWeek.Add("3", System.DayOfWeek.Tuesday);
            dayOfWeekStringToDayOfWeek.Add("4", System.DayOfWeek.Wednesday);
            dayOfWeekStringToDayOfWeek.Add("5", System.DayOfWeek.Thursday);
            dayOfWeekStringToDayOfWeek.Add("6", System.DayOfWeek.Friday);
            dayOfWeekStringToDayOfWeek.Add("7", System.DayOfWeek.Saturday);

            dayOfWeekToInt = new Dictionary<System.DayOfWeek, int>();
            dayOfWeekToInt.Add(System.DayOfWeek.Sunday, 1);
            dayOfWeekToInt.Add(System.DayOfWeek.Monday, 2);
            dayOfWeekToInt.Add(System.DayOfWeek.Tuesday, 3);
            dayOfWeekToInt.Add(System.DayOfWeek.Wednesday, 4);
            dayOfWeekToInt.Add(System.DayOfWeek.Thursday, 5);
            dayOfWeekToInt.Add(System.DayOfWeek.Friday, 6);
            dayOfWeekToInt.Add(System.DayOfWeek.Saturday, 7);

            intToDayOfWeek = new Dictionary<int, System.DayOfWeek>();
            intToDayOfWeek.Add(1, System.DayOfWeek.Sunday);
            intToDayOfWeek.Add(2, System.DayOfWeek.Monday);
            intToDayOfWeek.Add(3, System.DayOfWeek.Tuesday);
            intToDayOfWeek.Add(4, System.DayOfWeek.Wednesday);
            intToDayOfWeek.Add(5, System.DayOfWeek.Thursday);
            intToDayOfWeek.Add(6, System.DayOfWeek.Friday);
            intToDayOfWeek.Add(7, System.DayOfWeek.Saturday);
        }

        public static DayOfWeek AsDayOfWeek(this int dayOfWeek) => intToDayOfWeek[dayOfWeek];

        public static int AsInt(this Month month) => (int)month;

        public static int AsInt(this DayOfWeek dayOfWeek) => dayOfWeekToInt[dayOfWeek];

        public static bool ContainsDayOfWeek(string dayOfWeek) => dayOfWeekStringToDayOfWeek.ContainsKey(dayOfWeek.ToLowerInvariant());

        public static bool ContainsMonth(string month) => monthsStringToMonth.ContainsKey(month.ToLowerInvariant());

        public static DayOfWeek DayOfWeek(string dayOfWeek) => dayOfWeekStringToDayOfWeek[dayOfWeek.ToLowerInvariant()];

        public static Month Month(string month) => monthsStringToMonth[month.ToLowerInvariant()];
    }
}
