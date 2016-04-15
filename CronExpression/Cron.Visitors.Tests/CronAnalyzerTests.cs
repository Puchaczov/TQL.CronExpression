using Cron.Visitors;
using Cron.Visitors.Evaluators;
using Cron.Visitors.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Cron.Parser.Tests
{
    [TestClass]
    public class CronAnalyzerTests
    {
        [TestMethod]
        public void TestWillFireEverySecond_ShouldPass()
        {
            var referenceTime = new DateTime(2000, 1, 1, 0, 0, 59);
            var analyzer = "* * * * * *".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 60, analyzer, (datetime, secondsToAdd) => {
                Assert.AreEqual(referenceTime.AddSeconds(secondsToAdd), datetime);
            });
        }

        [TestMethod]
        public void TestWillFireEveryMinute_ShouldPass()
        {
            var referenceTime = new DateTime(2000, 1, 1, 0, 59, 0);
            var analyzer = "0 * * * * *".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 60, analyzer, (datetime, minutesToAdd) => {
                Assert.AreEqual(referenceTime.AddMinutes(minutesToAdd), datetime);
            });
        }

        [TestMethod]
        public void TestWillFireEveryHour_ShouldPass()
        {
            var referenceTime = new DateTime(2000, 1, 1, 23, 0, 0);
            var analyzer = "0 0 * * * *".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 60, analyzer, (datetime, hoursToAdd) => {
                Assert.AreEqual(referenceTime.AddHours(hoursToAdd), datetime);
            });
        }

        [TestMethod]
        public void TestWillFireEveryDay_ShouldPass()
        {
            var referenceTime = new DateTime(2000, 1, 1, 0, 0, 0);
            var analyzer = "0 0 0 * * *".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 60, analyzer, (datetime, daysToAdd) => {
                Assert.AreEqual(referenceTime.AddDays(daysToAdd), datetime);
            });
        }

        [TestMethod]
        public void TestWillFireAt5thDayOfEveryMonth_ShouldPass()
        {
            var referenceTime = new DateTime(2016, 1, 5, 0, 0, 0);
            var analyzer = "0 0 0 5 * *".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 12, analyzer, (datetime, monthsToAdd) => {
                Assert.AreEqual(referenceTime.AddMonths(monthsToAdd), datetime);
            });
        }

        [TestMethod]
        public void TestWillFireAtNewYear_WithFullOverflow_ShouldPass()
        {
            var referenceTime = new DateTime(2016, 12, 31, 23, 59, 59);
            var analyzer = "* * * * * *".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 2, analyzer, (datetime, monthsToAdd) => {
                Assert.AreEqual(referenceTime.AddSeconds(monthsToAdd), datetime);
            });
        }

        [TestMethod]
        public void TestWillFireInSpecificDayOfMonth_ShouldPass()
        {
            var referenceTime = new DateTime(2016, 1, 1, 0, 0, 0);
            var analyzer = "0 0 0 * * MON".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            Assert.AreEqual(new DateTime(2016, 1, 4, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 1, 11, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 1, 18, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 1, 25, 0, 0, 0), analyzer.NextFire());

            Assert.AreEqual(new DateTime(2016, 2, 1, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 2, 8, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 2, 15, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 2, 22, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 2, 29, 0, 0, 0), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireInSpecificRangeOfDayOfMonths_ShouldPass()
        {
            var referenceTime = new DateTime(2016, 1, 1, 0, 0, 0);
            var analyzer = "0 0 0 * * MON-WED".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            Assert.AreEqual(new DateTime(2016, 1, 4, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 1, 5, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 1, 6, 0, 0, 0), analyzer.NextFire());

            Assert.AreEqual(new DateTime(2016, 1, 11, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 1, 12, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 1, 13, 0, 0, 0), analyzer.NextFire());

            Assert.AreEqual(new DateTime(2016, 1, 18, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 1, 19, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 1, 20, 0, 0, 0), analyzer.NextFire());

            Assert.AreEqual(new DateTime(2016, 1, 25, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 1, 26, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 1, 27, 0, 0, 0), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireInThirdWednesdayOfMonth_ShouldPass()
        {
            var referenceTime = new DateTime(2016, 1, 1, 0, 0, 0);
            var analyzer = "0 0 0 * * 4#3".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            Assert.AreEqual(new DateTime(2016, 1, 20, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 2, 17, 0, 0, 0), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireInSpecificYearRange_ShouldPass()
        {
            var referenceTime = new DateTime(2015, 1, 1, 0, 0, 0);
            var analyzer = "0 0 0 1 1 * 2016-2017".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            Assert.AreEqual(new DateTime(2016, 1, 1, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2017, 1, 1, 0, 0, 0), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireOnlyInLeapYears_ShouldPass()
        {
            var referenceTime = new DateTime(2015, 1, 1, 0, 0, 0);
            var analyzer = "0 0 0 29 2 * 2015-2045".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            Assert.AreEqual(new DateTime(2016, 2, 29, 0, 0, 0), analyzer.NextFire());
            CheckNextFireExecutionTimeForSpecificPartOfDateTime(0, 20, 4, analyzer, (time, years) => {
                Assert.AreEqual(new DateTime(2020, 2, 29, 0, 0, 0).AddYears(years), time);
            });
        }

        [TestMethod]
        public void TestWillFireWithSpecificIncreasedSeconds_ShouldPass()
        {
            var referenceTime = new DateTime(2015, 1, 1, 0, 0, 0);
            var analyzer = "0/12 * 0 1 1 * *".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 0, 12), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 0, 24), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 0, 36), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 0, 48), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 1, 0), analyzer.NextFire());
        }

        private static void CheckNextFireExecutionTimeForSpecificPartOfDateTime(int from, int to, ICronFireTimeEvaluator analyzer, Action<DateTime, int> assertCallback)
        {
            CheckNextFireExecutionTimeForSpecificPartOfDateTime(from, to, 1, analyzer, assertCallback);
        }

        private static void CheckNextFireExecutionTimeForSpecificPartOfDateTime(int from, int to, int inc, ICronFireTimeEvaluator analyzer, Action<DateTime, int> assertCallback)
        {
            if(assertCallback == null)
            {
                throw new ArgumentNullException(nameof(assertCallback));
            }

            for (int i = from; i < to; i += inc)
            {
                assertCallback(analyzer.NextFire().Value, i);
            }
        }

        [TestMethod]
        public void TestWillRunEvery20thSecondOfEveryMinutes_ShouldPass()
        {
            var analyzer = "20 * * * * *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2000, 1, 1, 0, 0, 59);
            var time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2000, 1, 1, 0, 1, 20), time);
            time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2000, 1, 1, 0, 2, 20), time);
            time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2000, 1, 1, 0, 3, 20), time);
        }

        [TestMethod]
        public void TestWillRunEvery1stMinuteOfEveryHour_ShouldPass()
        {
            var analyzer = "0 1 * * * *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2000, 1, 1, 0, 0, 58);
            var time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2000, 1, 1, 0, 1, 0), time);
            time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2000, 1, 1, 1, 1, 0), time);
            time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2000, 1, 1, 2, 1, 0), time);
        }

        [TestMethod]
        public void TestWillRunIn1stAnd2ndSecondsInFirstMinuteOfEveryHour_ShouldPass()
        {
            var analyzer = "0,1 1 * * * *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2000, 1, 1, 0, 0, 58);
            var time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2000, 1, 1, 0, 1, 0), time);
            time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2000, 1, 1, 0, 1, 1), time);
            time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2000, 1, 1, 1, 1, 0), time);
            time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2000, 1, 1, 1, 1, 1), time);
        }

        [TestMethod]
        public void TestWillRunInFirstDayOfFirstMonthEveryYear_ShouldPass()
        {
            var analyzer = "0 0 0 1 1 *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2000, 1, 1, 0, 0, 58);
            var time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2001, 1, 1, 0, 0, 0), time);
            time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2002, 1, 1, 0, 0, 0), time);
        }

        [TestMethod]
        public void TestWillRunOnlyTwiceInSpecificYears_ShouldPass()
        {
            var analyzer = "13 25 15 1 1 ? 2016,2017".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2000, 1, 1, 0, 0, 58);
            var time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2016, 1, 1, 15, 25, 13), time);
            time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2017, 1, 1, 15, 25, 13), time);
        }

        [TestMethod]
        public void TestWillRunAtSpecificHourEveryDaysInMonth_ShouldPass()
        {
            var analyzer = "0 15 23 * * ?".TakeEvaluator();

            var refTime = new DateTime(2005, 6, 1, 23, 16, 0);
            analyzer.ReferenceTime = refTime;
            var expectedFireTime = new DateTime(2005, 6, 2, 23, 15, 0);
            var propFireTime = analyzer.NextFire();
            Assert.AreEqual(expectedFireTime, propFireTime);
            expectedFireTime = new DateTime(2005, 6, 3, 23, 15, 0);
            Assert.AreEqual(expectedFireTime, analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillRunAt2ndHourAtNightEveryDay_ShouldPass()
        {
            var analyzer = "* * 1 * * ?".TakeEvaluator();
            DateTimeOffset cal = new DateTime(2005, 7, 31, 22, 59, 57).ToUniversalTime();
            DateTimeOffset nextExpectedFireTime = new DateTime(2005, 8, 1, 1, 0, 0).ToUniversalTime();
            analyzer.OffsetReferenceTime = cal;
            var value = analyzer.NextFire();
            Assert.AreEqual(nextExpectedFireTime, value.Value.ToUniversalTime());
        }

        [TestMethod]
        public void TestFullOverflowedDate_ShouldPass()
        {
            var analyzer = "* * * * * * *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2005, 12, DateTime.DaysInMonth(2005, 12), 23, 59, 59);
            Assert.AreEqual(new DateTime(2006, 1, 1, 0, 0, 0), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireInTheLastDayOfEveryMonth_ShouldPass()
        {
            var analyzer = "0 15 10 L * ?".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2016, 1, 1, 0, 0, 0);
            var expectedFireTime = new DateTime(2016, 1, 31, 10, 15, 0);
            Assert.AreEqual(expectedFireTime, analyzer.NextFire());
            expectedFireTime = new DateTime(2016, 2, 29, 10, 15, 0);
            Assert.AreEqual(expectedFireTime, analyzer.NextFire());
        }

        [TestMethod]
        public void TestTheLast6thDayOfMonth_ShouldPass()
        {
            var analyzer = "0 0 0 5L * ?".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2016, 1, 1, 0, 0, 0);
            var expectedFireTime = new DateTime(2016, 1, 26, 0, 0, 0);
            Assert.AreEqual(expectedFireTime, analyzer.NextFire());
        }

        [TestMethod]
        public void TestTheLastFridayOfMonth_ShouldPass()
        {
            var analyzer = "0 2 0 ? * 6L 2016".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2016, 1, 1, 0, 0, 0);
            var expectedFireTime = new DateTime(2016, 1, 29, 0, 2, 0);
            Assert.AreEqual(expectedFireTime, analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireThirdFridayOfEveryMonth_ShouldPass()
        {
            var analyzer = "0 15 10 ? * 6#3".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2016, 1, 1);
            var expectedFireTime = new DateTime(2016, 1, 15, 10, 15, 0);
            Assert.AreEqual(expectedFireTime, analyzer.NextFire());
            expectedFireTime = new DateTime(2016, 2, 19, 10, 15, 0);
            Assert.AreEqual(expectedFireTime, analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireInEveryWeekday_ShouldPass()
        {
            var analyzer = "0 0 0 * * 2-6 *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2015, 12, DateTime.DaysInMonth(2015, 12), 23, 59, 0);
            Assert.AreEqual(new DateTime(2016, 1, 1), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 1, 4), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 1, 5), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 1, 6), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 1, 7), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 1, 8), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 1, 11), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireNearWeekday_ShouldPass()
        {
            var analyzer = "0 0 0 W * * *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2016, 2, 29);
            Assert.AreEqual(new DateTime(2016, 3, 1), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 4, 1), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 5, 2), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireNearWeekday_NumericPrecedeed_ShouldPass()
        {
            var analyzer = "0 0 0 1W * * *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2016, 2, 29);
            Assert.AreEqual(new DateTime(2016, 3, 1), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 4, 1), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 5, 2), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireNearWeekday_LastDayInMonth_ShouldPass()
        {
            var analyzer = "0 0 0 31W * * *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2016, 1, 1);
            Assert.AreEqual(new DateTime(2016, 1, 29), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 3, 31), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireInTheLastWeekdayOfMonth_ShouldPass()
        {
            var analyzer = "0 0 0 LW * * *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2016, 1, 1);
            Assert.AreEqual(new DateTime(2016, 1, 29), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 2, 29), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 3, 31), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillReturnNullWhenExpressionExceedTimeBoundary_ShouldReturnNull()
        {
            var analyzer = "0 0 1 * * * 2015".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2016, 1, 1);
            Assert.IsNull(analyzer.NextFire());
        }
    }
}
