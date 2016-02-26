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
            var analyzer = "* * * * * *".AsCronExpression();
            analyzer.ReferenceTime = referenceTime;
            
            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 60, analyzer, (datetime, secondsToAdd) => {
                Assert.AreEqual(referenceTime.AddSeconds(secondsToAdd), datetime);
            });
        }

        [TestMethod]
        public void TestWillFireEveryMinute_ShouldPass()
        {
            var referenceTime = new DateTime(2000, 1, 1, 0, 59, 0);
            var analyzer = "0 * * * * *".AsCronExpression();
            analyzer.ReferenceTime = referenceTime;

            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 60, analyzer, (datetime, minutesToAdd) => {
                Assert.AreEqual(referenceTime.AddMinutes(minutesToAdd), datetime);
            });
        }

        [TestMethod]
        public void TestWillFireEveryHour_ShouldPass()
        {
            var referenceTime = new DateTime(2000, 1, 1, 23, 0, 0);
            var analyzer = "0 0 * * * *".AsCronExpression();
            analyzer.ReferenceTime = referenceTime;

            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 60, analyzer, (datetime, hoursToAdd) => {
                Assert.AreEqual(referenceTime.AddHours(hoursToAdd), datetime);
            });
        }

        [TestMethod]
        public void TestWillFireEveryDay_ShouldPass()
        {
            var referenceTime = new DateTime(2000, 1, 1, 0, 0, 0);
            var analyzer = "0 0 0 * * *".AsCronExpression();
            analyzer.ReferenceTime = referenceTime;

            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 60, analyzer, (datetime, daysToAdd) => {
                Assert.AreEqual(referenceTime.AddDays(daysToAdd), datetime);
            });
        }

        [TestMethod]
        public void TestWillFireAt5thDayOfEveryMonth_ShouldPass()
        {
            var referenceTime = new DateTime(2016, 1, 5, 0, 0, 0);
            var analyzer = "0 0 0 5 * *".AsCronExpression();
            analyzer.ReferenceTime = referenceTime;

            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 12, analyzer, (datetime, monthsToAdd) => {
                Assert.AreEqual(referenceTime.AddMonths(monthsToAdd), datetime);
            });
        }

        [TestMethod]
        public void TestWillFireAtNewYear_WithFullOverflow_ShouldPass()
        {
            var referenceTime = new DateTime(2016, 12, 31, 23, 59, 59);
            var analyzer = "* * * * * *".AsCronExpression();
            analyzer.ReferenceTime = referenceTime;

            CheckNextFireExecutionTimeForSpecificPartOfDateTime(1, 2, analyzer, (datetime, monthsToAdd) => {
                Assert.AreEqual(referenceTime.AddSeconds(monthsToAdd), datetime);
            });
        }

        [TestMethod]
        public void TestWillFireInSpecificDayOfMonth_ShouldPass()
        {
            var referenceTime = new DateTime(2016, 1, 1, 0, 0, 0);
            var analyzer = "0 0 0 * * MON".AsCronExpression();
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
            var analyzer = "0 0 0 * * MON-WED".AsCronExpression();
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
            var analyzer = "0 0 0 * * 4#3".AsCronExpression();
            analyzer.ReferenceTime = referenceTime;

            Assert.AreEqual(new DateTime(2016, 1, 20, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2016, 2, 17, 0, 0, 0), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireInSpecificYearRange_ShouldPass()
        {
            var referenceTime = new DateTime(2015, 1, 1, 0, 0, 0);
            var analyzer = "0 0 0 1 1 * 2016-2017".AsCronExpression();
            analyzer.ReferenceTime = referenceTime;

            Assert.AreEqual(new DateTime(2016, 1, 1, 0, 0, 0), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2017, 1, 1, 0, 0, 0), analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireOnlyInLeapYears_ShouldPass()
        {
            var referenceTime = new DateTime(2015, 1, 1, 0, 0, 0);
            var analyzer = "0 0 0 29 2 * 2015-2045".AsCronExpression();
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
            var analyzer = "0/12 * 0 1 1 * *".AsCronExpression();
            analyzer.ReferenceTime = referenceTime;

            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 0, 12), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 0, 24), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 0, 36), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 0, 48), analyzer.NextFire());
            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 1, 0), analyzer.NextFire());
        }

        private void CheckNextFireExecutionTimeForSpecificPartOfDateTime(int from, int to, ICronFireTimeEvaluator analyzer, Action<DateTime, int> assertCallback)
        {
            CheckNextFireExecutionTimeForSpecificPartOfDateTime(from, to, 1, analyzer, assertCallback);
        }
        
        private void CheckNextFireExecutionTimeForSpecificPartOfDateTime(int from, int to, int inc, ICronFireTimeEvaluator analyzer, Action<DateTime, int> assertCallback)
        {
            for (int i = from; i < to; i += inc)
            {
                assertCallback(analyzer.NextFire(), i);
            }
        }

        [TestMethod]
        public void TestExpression2()
        {
            var analyzer = "20 * * * * *".AsCronExpression();
            analyzer.ReferenceTime = new DateTime(2000, 1, 1, 0, 0, 59);
            var time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2000, 1, 1, 0, 1, 20), time);
            time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2000, 1, 1, 0, 2, 20), time);
            time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2000, 1, 1, 0, 3, 20), time);
        }

        [TestMethod]
        public void TestExpression3()
        {
            var analyzer = "0 1 * * * *".AsCronExpression();
            analyzer.ReferenceTime = new DateTime(2000, 1, 1, 0, 0, 58);
            var time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2000, 1, 1, 0, 1, 0), time);
            time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2000, 1, 1, 1, 1, 0), time);
            time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2000, 1, 1, 2, 1, 0), time);
        }

        [TestMethod]
        public void TestExpression4()
        {
            var analyzer = "0,1 1 * * * *".AsCronExpression();
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
        public void TestExpression5()
        {
            var analyzer = "0 0 0 1 1 *".AsCronExpression();
            analyzer.ReferenceTime = new DateTime(2000, 1, 1, 0, 0, 58);
            var time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2001, 1, 1, 0, 0, 0), time);
            time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2002, 1, 1, 0, 0, 0), time);
        }

        [TestMethod]
        public void TestExpression6()
        {
            var analyzer = "13 25 15 1 1 ? 2016,2017".AsCronExpression();
            analyzer.ReferenceTime = new DateTime(2000, 1, 1, 0, 0, 58);
            var time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2016, 1, 1, 15, 25, 13), time);
            time = analyzer.NextFire();
            Assert.AreEqual(new DateTime(2017, 1, 1, 15, 25, 13), time);
        }

        [TestMethod]
        public void TestExpression7()
        {
            var analyzer = "0 15 23 * * ?".AsCronExpression();

            var refTime = new DateTime(2005, 6, 1, 23, 16, 0);
            analyzer.ReferenceTime = refTime;
            var expectedFireTime = new DateTime(2005, 6, 2, 23, 15, 0);
            var propFireTime = analyzer.NextFire();
            Assert.AreEqual(expectedFireTime, propFireTime);
        }

        [TestMethod]
        public void TestExpression8()
        {
            var analyzer = "* * 1 * * ?".AsCronExpression();
            DateTimeOffset cal = new DateTime(2005, 7, 31, 22, 59, 57).ToUniversalTime();
            DateTimeOffset nextExpectedFireTime = new DateTime(2005, 8, 1, 1, 0, 0).ToUniversalTime();
            analyzer.OffsetReferenceTime = cal;
            var value = analyzer.NextFire();
            Assert.AreEqual(nextExpectedFireTime, value);
        }

        [TestMethod]
        public void TestFullOverflowedDate()
        {
            var analyzer = "* * * * * * *".AsCronExpression();
            analyzer.ReferenceTime = new DateTime(2005, 12, DateTime.DaysInMonth(2005, 12), 23, 59, 59);
            var expectedFireTime = new DateTime(2006, 1, 1, 0, 0, 0);
            Assert.AreEqual(expectedFireTime, analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireInTheLastDayOfEveryMonth()
        {
            var analyzer = "0 15 10 L * ?".AsCronExpression();
            analyzer.ReferenceTime = new DateTime(2016, 1, 1, 0, 0, 0);
            var expectedFireTime = new DateTime(2016, 1, 31, 10, 15, 0);
            Assert.AreEqual(expectedFireTime, analyzer.NextFire());
            expectedFireTime = new DateTime(2016, 2, 29, 10, 15, 0);
            Assert.AreEqual(expectedFireTime, analyzer.NextFire());
        }

        [TestMethod]
        public void TestTheLast6thDayOfMonth()
        {
            var analyzer = "0 0 0 5L * ?".AsCronExpression();
            analyzer.ReferenceTime = new DateTime(2016, 1, 1, 0, 0, 0);
            var expectedFireTime = new DateTime(2016, 1, 26, 0, 0, 0);
            Assert.AreEqual(expectedFireTime, analyzer.NextFire());
        }

        [TestMethod]
        public void TestTheLastFridayOfMonth()
        {
            var analyzer = "0 2 0 ? * 6L 2016".AsCronExpression();
            analyzer.ReferenceTime = new DateTime(2016, 1, 1, 0, 0, 0);
            var expectedFireTime = new DateTime(2016, 1, 29, 0, 2, 0);
            Assert.AreEqual(expectedFireTime, analyzer.NextFire());
        }

        [TestMethod]
        public void TestWillFireThirdFridayOfEveryMonth()
        {
            var analyzer = "0 15 10 ? * 6#3".AsCronExpression();
            analyzer.ReferenceTime = new DateTime(2016, 1, 1);
            var expectedFireTime = new DateTime(2016, 1, 15, 10, 15, 0);
            Assert.AreEqual(expectedFireTime, analyzer.NextFire());
            expectedFireTime = new DateTime(2016, 2, 19, 10, 15, 0);
            Assert.AreEqual(expectedFireTime, analyzer.NextFire());
        }
    }
}
