using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TQL.CronExpression.TimelineEvaluator.Evaluators;
using TQL.CronExpression.TimelineEvaluator.Helpers;

namespace TQL.CronExpression.Parser.Tests
{
    [TestClass]
    public class CronAnalyzerTests
    {
        [TestMethod]
        public void TestFullOverflowedDate_ShouldPass()
        {
            var analyzer = "* * * * * * *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2005, 12, DateTime.DaysInMonth(2005, 12), 23, 59, 59);
            Assert.AreEqual(new DateTime(2006, 1, 1, 0, 0, 0), analyzer.NextFire());
        }

        [TestMethod]
        public void TestIsSatisfiedByWillReturnFalseWhenPastTimePassed_ShouldReturnFalse()
        {
            var analyzer = "12 15 1 * * * 2015".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2015, 1, 1, 0, 0, 0);
            Assert.IsFalse(analyzer.IsSatisfiedBy(new DateTime(2014, 1, 1, 0, 0, 0)));
        }

        [TestMethod]
        public void TestIsSatisfiedByWillReturnNullWhenTimeBoundaryExceed_ShouldReturnFalse()
        {
            var analyzer = "12 15 1 * * * 2015".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2016, 1, 1, 0, 0, 0);
            Assert.IsFalse(analyzer.IsSatisfiedBy(new DateTime(2016, 1, 1, 1, 15, 12)));
        }

        [TestMethod]
        public void TestIsSatisfiedByWillReturnTrueWhenTimesMatched_ShouldReturnTrue()
        {
            var analyzer = "12 15 1 * * * 2015".TakeEvaluator();

            analyzer.ReferenceTime = new DateTime(2015, 1, 1, 0, 0, 0);
            Assert.IsTrue(analyzer.IsSatisfiedBy(new DateTime(2015, 1, 1, 1, 15, 12)));

            analyzer.ReferenceTime = new DateTime(2015, 5, 1, 0, 0, 0);
            Assert.IsTrue(analyzer.IsSatisfiedBy(new DateTime(2015, 5, 1, 1, 15, 12)));
        }

        [TestMethod]
        public void TestWillBeSatisfiedWhenMillisecondsDateTime_ShouldPass()
        {
            var analyzer = "* * 12 ? * *".TakeEvaluator();
            var oneSecondLater = new DateTime(2012, 1, 1, 12, 0, 1).AddMilliseconds(15);
            analyzer.ReferenceTime = new DateTime(2012, 1, 1, 12, 0, 0);
            Assert.IsTrue(analyzer.IsSatisfiedBy(oneSecondLater));
        }

        [TestMethod]
        public void TestIsSatisfiedBy_ShouldPass()
        {
            var analyzer = "0 15 10 * * ? 2005".TakeEvaluator();

            var cal = new DateTime(2005, 6, 1, 10, 15, 0).ToUniversalTime();
            analyzer.ReferenceTime = cal;

            cal = cal.AddYears(1);
            Assert.IsFalse(analyzer.IsSatisfiedBy(cal));

            cal = new DateTime(2005, 6, 1, 10, 16, 0).ToUniversalTime();
            Assert.IsFalse(analyzer.IsSatisfiedBy(cal));

            cal = new DateTime(2005, 6, 1, 10, 14, 0).ToUniversalTime();
            Assert.IsFalse(analyzer.IsSatisfiedBy(cal));

        }

        [TestMethod]
        public void TestIsSatysfiedByWeekends_ShouldPass()
        {
            var analyzer = "0 15 10 ? * MON-FRI".TakeEvaluator();

            var cal = new DateTime(2007, 6, 9, 10, 15, 0).ToUniversalTime();
            analyzer.ReferenceTime = cal;
            Assert.IsFalse(analyzer.IsSatisfiedBy(cal));
            Assert.IsFalse(analyzer.IsSatisfiedBy(cal.AddDays(1)));
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
        public void TestWillFireAt5thDayOfEveryMonth_ShouldPass()
        {
            var referenceTime = new DateTimeOffset(2016, 1, 5, 0, 0, 0, TimeSpan.Zero);
            var analyzer = "0 0 0 5 * *".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 12, analyzer, (datetime, monthsToAdd) =>
            {
                var val1 = (DateTimeOffset)referenceTime.AddMonths(monthsToAdd);
                var val2 = datetime;
                Assert.AreEqual(val1, val2);
            });
        }

        [TestMethod]
        public void TestWillFireAtNewYear_WithFullOverflow_ShouldPass()
        {
            var referenceTime = new DateTime(2016, 12, 31, 23, 59, 59);
            var analyzer = "* * * * * *".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 2, analyzer, (datetime, monthsToAdd) =>
            {
                Assert.AreEqual(referenceTime.AddSeconds(monthsToAdd), datetime);
            });
        }

        [TestMethod]
        public void TestWillFireEveryDay_ShouldPass()
        {
            var referenceTime = new DateTime(2000, 1, 1, 0, 0, 0);
            var analyzer = "0 0 0 * * *".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 60, analyzer, (datetime, daysToAdd) =>
            {
                Assert.AreEqual(referenceTime.AddDays(daysToAdd), datetime);
            });
        }

        [TestMethod]
        public void TestWillFireEveryHour_ShouldPass()
        {
            var referenceTime = new DateTime(2000, 1, 1, 23, 0, 0);
            var analyzer = "0 0 * * * *".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 60, analyzer, (datetime, hoursToAdd) =>
            {
                Assert.AreEqual(referenceTime.AddHours(hoursToAdd), datetime);
            });
        }

        [TestMethod]
        public void TestWillFireEveryMinute_ShouldPass()
        {
            var referenceTime = new DateTime(2000, 1, 1, 0, 59, 0);
            var analyzer = "0 * * * * *".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 60, analyzer, (datetime, minutesToAdd) =>
            {
                Assert.AreEqual(referenceTime.AddMinutes(minutesToAdd), datetime);
            });
        }
        [TestMethod]
        public void TestWillFireEverySecond_ShouldPass()
        {
            var referenceTime = new DateTime(2000, 1, 1, 0, 0, 59);
            var analyzer = "* * * * * *".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 60, analyzer, (datetime, secondsToAdd) =>
            {
                Assert.AreEqual(referenceTime.AddSeconds(secondsToAdd), datetime);
            });
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
        public void TestWillFireInSpecificYearRange_ShouldPass()
        {
            var referenceTime = new DateTime(2015, 1, 1, 0, 0, 0);
            var analyzer = "0 0 0 1 1 * 2016-2017".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            Assert.AreEqual(new DateTime(2016, 1, 1, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2017, 1, 1, 0, 0, 0), analyzer.NextFire());
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
        public void TestWillFireInTheLastWeekdayOfMonth_ShouldPass()
        {
            var analyzer = "0 0 0 LW * * *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTimeOffset(2016, 1, 1, 0, 0, 0, TimeSpan.Zero);
            Assert.AreEqual(new DateTimeOffset(2016, 1, 29, 0, 0, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 2, 29, 0, 0, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 3, 31, 0, 0, 0, TimeSpan.Zero), analyzer.NextFire());
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
        public void TestWillFireNearWeekday_LastDayInMonth_ShouldPass()
        {
            var analyzer = "0 0 0 31W * * *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTimeOffset(2016, 1, 1, 0, 0, 0, TimeSpan.Zero);
            Assert.AreEqual(new DateTimeOffset(2016, 1, 29, 0, 0, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 3, 31, 0, 0, 0, TimeSpan.Zero), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireNearWeekday_NumericPrecedeed_ShouldPass()
        {
            var analyzer = "0 0 0 1W * * *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTimeOffset(2016, 2, 29, 0, 0, 0, TimeSpan.Zero);
            Assert.AreEqual(new DateTimeOffset(2016, 3, 1, 0, 0, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 4, 1, 0, 0, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 5, 2, 0, 0, 0, TimeSpan.Zero), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireNearWeekday_ShouldPass()
        {
            var analyzer = "0 0 0 W * * *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTimeOffset(2016, 2, 29, 0, 0, 0, TimeSpan.Zero);
            Assert.AreEqual(new DateTimeOffset(2016, 3, 1, 0, 0, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 4, 1, 0, 0, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 5, 2, 0, 0, 0, TimeSpan.Zero), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireOnlyInLeapYears_ShouldPass()
        {
            var referenceTime = new DateTime(2015, 1, 1, 0, 0, 0);
            var analyzer = "0 0 0 29 2 * 2015-2045".TakeEvaluator();
            analyzer.ReferenceTime = referenceTime;

            Assert.AreEqual(new DateTime(2016, 2, 29, 0, 0, 0), analyzer.NextFire());
            CheckNextFireExecutionTimeForSpecificPartOfDateTime(0, 20, 4, analyzer, (time, years) =>
            {
                Assert.AreEqual(new DateTime(2020, 2, 29, 0, 0, 0).AddYears(years), time);
            });
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

        [TestMethod]
        public void TestWillReturnNullWhenExpressionExceedTimeBoundary_ShouldReturnNull()
        {
            var analyzer = "0 0 1 * * * 2015".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2016, 1, 1);
            Assert.IsNull(analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillRunAt2ndHourAtNightEveryDay_ShouldPass()
        {
            var analyzer = "* * 1 * * ?".TakeEvaluator();
            DateTimeOffset cal = new DateTimeOffset(2005, 7, 31, 22, 59, 57, TimeSpan.Zero);
            DateTimeOffset nextExpectedFireTime = new DateTimeOffset(2005, 8, 1, 1, 0, 0, TimeSpan.Zero);
            analyzer.ReferenceTime = cal;
            var value = analyzer.NextFire();
            Assert.AreEqual(nextExpectedFireTime, value);
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
        public void TestWillRunEveryMonday_ShouldPass()
        {
            var analyzer = "0 0 8 * * 1 *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2014, 11, 30); //sunday
            Assert.AreEqual(new DateTime(2014, 12, 1, 8, 0, 0), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillReturnNullWhenTimeExpired_ShouldReturnNull()
        {
            var analyzer = "* * * * * * 2010".TakeEvaluator();
            analyzer.ReferenceTime = new DateTime(2011, 1, 1);

            Assert.IsFalse(analyzer.NextFire().HasValue);
            Assert.IsFalse(analyzer.NextFire().HasValue);
            Assert.IsFalse(analyzer.NextFire().HasValue);
        }

        [TestMethod]
        public void TestWillFireEveryLastDayOfFebruary_ShouldPass()
        {
            var referenceTime = new DateTime(2015, 1, 1);
            var analyzer = "0 0 0 L 2 * 2015-2150".TakeEvaluator();

            analyzer.ReferenceTime = referenceTime;

            Assert.AreEqual(new DateTime(2015, 2, 28), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 2, 29), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2017, 2, 28), analyzer.NextFire());
        }

        [TestCategory("Not implemented features")]
        [Ignore()]
        [TestMethod]
        public void TestLastDayOffset()
        {
            var analyzer = "0 15 10 L-2 * ? 2010".TakeEvaluator();
            analyzer.ReferenceTime = new DateTimeOffset(2010, 1, 1, 0, 0, 0, TimeSpan.Zero);
            Assert.IsTrue(analyzer.IsSatisfiedBy(new DateTimeOffset(2010, 10, 29, 10, 15, 0, TimeSpan.Zero)));

            Assert.IsFalse(analyzer.IsSatisfiedBy(new DateTimeOffset(2010, 10, 28, 10, 15, 0, TimeSpan.Zero)));

            analyzer = "0 15 10 L-5W * ? 2010".TakeEvaluator();
            analyzer.ReferenceTime = new DateTimeOffset(2010, 1, 1, 0, 0, 0, TimeSpan.Zero);
            Assert.IsTrue(analyzer.IsSatisfiedBy(new DateTimeOffset(2010, 10, 26, 10, 15, 0, TimeSpan.Zero)));
        }

        [TestMethod]
        public void TestMinuteShift_ShouldPass()
        {
            var analyzer = "0/22 * * * * ? *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTimeOffset(2015, 1, 1, 0, 0, 0, TimeSpan.Zero);
            Assert.AreEqual(new DateTimeOffset(2015, 1, 1, 0, 0, 22, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2015, 1, 1, 0, 0, 44, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2015, 1, 1, 0, 1, 0, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2015, 1, 1, 0, 1, 22, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2015, 1, 1, 0, 1, 44, 0, TimeSpan.Zero), analyzer.NextFire());
        }

        [TestMethod]
        public void TestNthWeekDay_ShouldPass()
        {
            var analyzer = "0 30 10-13 ? * FRI#3 *".TakeEvaluator();
            DateTimeOffset start = new DateTimeOffset(2016, 5, 13, 0, 0, 0, TimeSpan.Zero);

            analyzer.ReferenceTime = start;

            Assert.AreEqual(new DateTimeOffset(2016, 5, 20, 10, 30, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 5, 20, 11, 30, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 5, 20, 12, 30, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 5, 20, 13, 30, 0, TimeSpan.Zero), analyzer.NextFire());

            Assert.AreEqual(new DateTimeOffset(2016, 6, 17, 10, 30, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 6, 17, 11, 30, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 6, 17, 12, 30, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 6, 17, 13, 30, 0, TimeSpan.Zero), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWithLMixedInMonthDays_ShouldPass()
        {
            var analyzer = "0 43 9 1,5,29,L,L * ? *".TakeEvaluator();
            analyzer.ReferenceTime = new DateTimeOffset(2016, 1, 1, 0, 0, 0, TimeSpan.Zero);

            Assert.AreEqual(new DateTimeOffset(2016, 1, 1, 9, 43, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 1, 5, 9, 43, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 1, 29, 9, 43, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 1, 31, 9, 43, 0, TimeSpan.Zero), analyzer.NextFire());

            Assert.AreEqual(new DateTimeOffset(2016, 2, 1, 9, 43, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 2, 5, 9, 43, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 2, 29, 9, 43, 0, TimeSpan.Zero), analyzer.NextFire());

            //Check double L doesn't generate few times same date.
            Assert.AreNotEqual(new DateTimeOffset(2016, 2, 29, 9, 43, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreNotEqual(new DateTimeOffset(2016, 2, 29, 9, 43, 0, TimeSpan.Zero), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWithLMixedInDayOfWeek_ShouldPass()
        {
            var analyzer = "0 43 9 ? * SAT,SUN,L".TakeEvaluator();
            analyzer.ReferenceTime = new DateTimeOffset(2016, 1, 1, 0, 0, 0, TimeSpan.Zero);

            Assert.AreEqual(new DateTimeOffset(2016, 1, 2, 9, 43, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 1, 3, 9, 43, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 1, 9, 9, 43, 0, TimeSpan.Zero), analyzer.NextFire());
            Assert.AreEqual(new DateTimeOffset(2016, 1, 10, 9, 43, 0, TimeSpan.Zero), analyzer.NextFire());

            //Check L doesn't generate few times same date.
            Assert.AreNotEqual(new DateTimeOffset(2016, 1, 10, 9, 43, 0, TimeSpan.Zero), analyzer.NextFire());
        }
        
        [TestMethod]
        public void TestWithInproperValuesInSegment_ShouldReturnNullEvaluator()
        {
            var evaluator = "*a * * * * * *".TakeEvaluator();
            Assert.IsNull(evaluator);
        }

        private static void CheckNextFireExecutionTimeForSpecificPartOfDateTime(int from, int to, ICronFireTimeEvaluator analyzer, Action<DateTimeOffset, int> assertCallback)
        {
            CheckNextFireExecutionTimeForSpecificPartOfDateTime(from, to, 1, analyzer, assertCallback);
        }

        private static void CheckNextFireExecutionTimeForSpecificPartOfDateTime(int from, int to, int inc, ICronFireTimeEvaluator analyzer, Action<DateTimeOffset, int> assertCallback)
        {
            if (assertCallback == null)
            {
                throw new ArgumentNullException(nameof(assertCallback));
            }

            for (int i = from; i < to; i += inc)
            {
                assertCallback(analyzer.NextFire().Value, i);
            }
        }
    }
}
