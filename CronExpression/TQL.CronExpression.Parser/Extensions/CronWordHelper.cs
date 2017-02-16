using System;
using System.Collections.Generic;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Extensions
{
    public static class CronWordHelper
    {
        private static readonly Dictionary<string, DayOfWeek> DayOfWeekStringToDayOfWeek;
        private static readonly Dictionary<DayOfWeek, int> DayOfWeekToInt;
        private static readonly Dictionary<int, DayOfWeek> IntToDayOfWeek;
        private static readonly Dictionary<string, Month> MonthsStringToMonth;

        static CronWordHelper()
        {
            MonthsStringToMonth = new Dictionary<string, Month>();
            MonthsStringToMonth.Add("january", Enums.Month.January);
            MonthsStringToMonth.Add("february", Enums.Month.February);
            MonthsStringToMonth.Add("march", Enums.Month.February);
            MonthsStringToMonth.Add("april", Enums.Month.April);
            MonthsStringToMonth.Add("may", Enums.Month.May);
            MonthsStringToMonth.Add("june", Enums.Month.June);
            MonthsStringToMonth.Add("july", Enums.Month.July);
            MonthsStringToMonth.Add("august", Enums.Month.August);
            MonthsStringToMonth.Add("september", Enums.Month.September);
            MonthsStringToMonth.Add("october", Enums.Month.October);
            MonthsStringToMonth.Add("november", Enums.Month.November);
            MonthsStringToMonth.Add("december", Enums.Month.December);

            MonthsStringToMonth.Add("jan", Enums.Month.January);
            MonthsStringToMonth.Add("feb", Enums.Month.February);
            MonthsStringToMonth.Add("mar", Enums.Month.March);
            MonthsStringToMonth.Add("ap", Enums.Month.April);
            MonthsStringToMonth.Add("jun", Enums.Month.June);
            MonthsStringToMonth.Add("jul", Enums.Month.July);
            MonthsStringToMonth.Add("aug", Enums.Month.August);
            MonthsStringToMonth.Add("sep", Enums.Month.September);
            MonthsStringToMonth.Add("oct", Enums.Month.October);
            MonthsStringToMonth.Add("nov", Enums.Month.November);
            MonthsStringToMonth.Add("dec", Enums.Month.December);

            MonthsStringToMonth.Add("1", Enums.Month.January);
            MonthsStringToMonth.Add("2", Enums.Month.February);
            MonthsStringToMonth.Add("3", Enums.Month.March);
            MonthsStringToMonth.Add("4", Enums.Month.April);
            MonthsStringToMonth.Add("5", Enums.Month.May);
            MonthsStringToMonth.Add("6", Enums.Month.June);
            MonthsStringToMonth.Add("7", Enums.Month.July);
            MonthsStringToMonth.Add("8", Enums.Month.August);
            MonthsStringToMonth.Add("9", Enums.Month.September);
            MonthsStringToMonth.Add("10", Enums.Month.October);
            MonthsStringToMonth.Add("11", Enums.Month.November);
            MonthsStringToMonth.Add("12", Enums.Month.December);

            DayOfWeekStringToDayOfWeek = new Dictionary<string, DayOfWeek>();
            DayOfWeekStringToDayOfWeek.Add("monday", System.DayOfWeek.Monday);
            DayOfWeekStringToDayOfWeek.Add("tuesday", System.DayOfWeek.Tuesday);
            DayOfWeekStringToDayOfWeek.Add("wednesday", System.DayOfWeek.Wednesday);
            DayOfWeekStringToDayOfWeek.Add("thursday", System.DayOfWeek.Thursday);
            DayOfWeekStringToDayOfWeek.Add("friday", System.DayOfWeek.Friday);
            DayOfWeekStringToDayOfWeek.Add("saturday", System.DayOfWeek.Saturday);
            DayOfWeekStringToDayOfWeek.Add("sunday", System.DayOfWeek.Sunday);

            DayOfWeekStringToDayOfWeek.Add("mon", System.DayOfWeek.Monday);
            DayOfWeekStringToDayOfWeek.Add("tue", System.DayOfWeek.Tuesday);
            DayOfWeekStringToDayOfWeek.Add("wed", System.DayOfWeek.Wednesday);
            DayOfWeekStringToDayOfWeek.Add("thu", System.DayOfWeek.Thursday);
            DayOfWeekStringToDayOfWeek.Add("fri", System.DayOfWeek.Friday);
            DayOfWeekStringToDayOfWeek.Add("sat", System.DayOfWeek.Saturday);
            DayOfWeekStringToDayOfWeek.Add("sun", System.DayOfWeek.Sunday);

            DayOfWeekStringToDayOfWeek.Add("1", System.DayOfWeek.Sunday);
            DayOfWeekStringToDayOfWeek.Add("2", System.DayOfWeek.Monday);
            DayOfWeekStringToDayOfWeek.Add("3", System.DayOfWeek.Tuesday);
            DayOfWeekStringToDayOfWeek.Add("4", System.DayOfWeek.Wednesday);
            DayOfWeekStringToDayOfWeek.Add("5", System.DayOfWeek.Thursday);
            DayOfWeekStringToDayOfWeek.Add("6", System.DayOfWeek.Friday);
            DayOfWeekStringToDayOfWeek.Add("7", System.DayOfWeek.Saturday);

            DayOfWeekToInt = new Dictionary<DayOfWeek, int>();
            DayOfWeekToInt.Add(System.DayOfWeek.Sunday, 1);
            DayOfWeekToInt.Add(System.DayOfWeek.Monday, 2);
            DayOfWeekToInt.Add(System.DayOfWeek.Tuesday, 3);
            DayOfWeekToInt.Add(System.DayOfWeek.Wednesday, 4);
            DayOfWeekToInt.Add(System.DayOfWeek.Thursday, 5);
            DayOfWeekToInt.Add(System.DayOfWeek.Friday, 6);
            DayOfWeekToInt.Add(System.DayOfWeek.Saturday, 7);

            IntToDayOfWeek = new Dictionary<int, DayOfWeek>();
            IntToDayOfWeek.Add(1, System.DayOfWeek.Sunday);
            IntToDayOfWeek.Add(2, System.DayOfWeek.Monday);
            IntToDayOfWeek.Add(3, System.DayOfWeek.Tuesday);
            IntToDayOfWeek.Add(4, System.DayOfWeek.Wednesday);
            IntToDayOfWeek.Add(5, System.DayOfWeek.Thursday);
            IntToDayOfWeek.Add(6, System.DayOfWeek.Friday);
            IntToDayOfWeek.Add(7, System.DayOfWeek.Saturday);
        }

        public static DayOfWeek AsDayOfWeek(this int dayOfWeek) => IntToDayOfWeek[dayOfWeek];

        public static int AsInt(this Month month) => (int) month;

        public static int AsInt(this DayOfWeek dayOfWeek) => DayOfWeekToInt[dayOfWeek];

        public static bool ContainsDayOfWeek(string dayOfWeek)
            => DayOfWeekStringToDayOfWeek.ContainsKey(dayOfWeek.ToLowerInvariant());

        public static bool ContainsMonth(string month) => MonthsStringToMonth.ContainsKey(month.ToLowerInvariant());

        public static DayOfWeek DayOfWeek(string dayOfWeek) => DayOfWeekStringToDayOfWeek[dayOfWeek.ToLowerInvariant()];

        public static Month Month(string month) => MonthsStringToMonth[month.ToLowerInvariant()];
    }
}