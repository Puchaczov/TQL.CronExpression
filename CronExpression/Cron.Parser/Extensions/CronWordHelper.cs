using Cron.Parser.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class CronWordHelper
    {
        private static Dictionary<string, Month> monthsStringToMonth;
        private static Dictionary<string, DayOfWeek> dayOfWeekStringToDayOfWeek;
        private static Dictionary<DayOfWeek, int> dayOfWeekToInt;
        private static Dictionary<int, DayOfWeek> intToDayOfWeek;

        static CronWordHelper()
        {
            monthsStringToMonth = new Dictionary<string, Month>();
            monthsStringToMonth.Add("january", Cron.Parser.Enums.Month.January);
            monthsStringToMonth.Add("february", Cron.Parser.Enums.Month.February);
            monthsStringToMonth.Add("march", Cron.Parser.Enums.Month.February);
            monthsStringToMonth.Add("april", Cron.Parser.Enums.Month.April);
            monthsStringToMonth.Add("may", Cron.Parser.Enums.Month.May);
            monthsStringToMonth.Add("june", Cron.Parser.Enums.Month.June);
            monthsStringToMonth.Add("july", Cron.Parser.Enums.Month.July);
            monthsStringToMonth.Add("august", Cron.Parser.Enums.Month.August);
            monthsStringToMonth.Add("september", Cron.Parser.Enums.Month.September);
            monthsStringToMonth.Add("october", Cron.Parser.Enums.Month.October);
            monthsStringToMonth.Add("november", Cron.Parser.Enums.Month.November);
            monthsStringToMonth.Add("december", Cron.Parser.Enums.Month.December);

            monthsStringToMonth.Add("jan", Cron.Parser.Enums.Month.January);
            monthsStringToMonth.Add("feb", Cron.Parser.Enums.Month.February);
            monthsStringToMonth.Add("mar", Cron.Parser.Enums.Month.March);
            monthsStringToMonth.Add("ap", Cron.Parser.Enums.Month.April);
            monthsStringToMonth.Add("jun", Cron.Parser.Enums.Month.June);
            monthsStringToMonth.Add("jul", Cron.Parser.Enums.Month.July);
            monthsStringToMonth.Add("aug", Cron.Parser.Enums.Month.August);
            monthsStringToMonth.Add("sep", Cron.Parser.Enums.Month.September);
            monthsStringToMonth.Add("oct", Cron.Parser.Enums.Month.October);
            monthsStringToMonth.Add("nov", Cron.Parser.Enums.Month.November);
            monthsStringToMonth.Add("dec", Cron.Parser.Enums.Month.December);

            monthsStringToMonth.Add("1", Cron.Parser.Enums.Month.January);
            monthsStringToMonth.Add("2", Cron.Parser.Enums.Month.February);
            monthsStringToMonth.Add("3", Cron.Parser.Enums.Month.March);
            monthsStringToMonth.Add("4", Cron.Parser.Enums.Month.April);
            monthsStringToMonth.Add("5", Cron.Parser.Enums.Month.May);
            monthsStringToMonth.Add("6", Cron.Parser.Enums.Month.June);
            monthsStringToMonth.Add("7", Cron.Parser.Enums.Month.July);
            monthsStringToMonth.Add("8", Cron.Parser.Enums.Month.August);
            monthsStringToMonth.Add("9", Cron.Parser.Enums.Month.September);
            monthsStringToMonth.Add("10", Cron.Parser.Enums.Month.October);
            monthsStringToMonth.Add("11", Cron.Parser.Enums.Month.November);
            monthsStringToMonth.Add("12", Cron.Parser.Enums.Month.December);

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

        public static Month Month(string month)
        {
            return monthsStringToMonth[month.ToLowerInvariant()];
        }

        public static int AsInt(this Month month)
        {
            return (int)month;
        }

        public static DayOfWeek DayOfWeek(string dayOfWeek)
        {
            return dayOfWeekStringToDayOfWeek[dayOfWeek.ToLowerInvariant()];
        }

        public static bool ContainsMonth(string month)
        {
            return monthsStringToMonth.ContainsKey(month.ToLowerInvariant());
        }

        public static bool ContainsDayOfWeek(string dayOfWeek)
        {
            return dayOfWeekStringToDayOfWeek.ContainsKey(dayOfWeek.ToLowerInvariant());
        }

        public static int AsInt(this DayOfWeek dayOfWeek)
        {
            return dayOfWeekToInt[dayOfWeek];
        }

        public static DayOfWeek AsDayOfWeek(this int dayOfWeek)
        {
            return intToDayOfWeek[dayOfWeek];
        }
    }
}
