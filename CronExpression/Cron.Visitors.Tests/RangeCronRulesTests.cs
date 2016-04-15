using Cron.Parser.Helpers;
using Cron.Parser.Tests;
using Cron.Parser.Visitors;
using Cron.Visitors;
using Cron.Visitors.Exceptions;
using Cron.Visitors.Helpers;
using Cron.Visitors.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Cron.Parser.Tests
{
    [TestClass]
    public class RangeCronRulesTests
    {

        [TestMethod]
        public void CheckRange_AllRangesAreIncorrect_ShouldAggregateExceptions()
        {
            var visitor = "1-100 60-120 24-48 60-45 13-23 7-15 0-5".TakeVisitor();
            Assert.AreEqual(false, visitor.IsValid);
            Assert.AreEqual(7, visitor.ValidationErrors.Count());
            Assert.AreEqual(typeof(RangeNodeException), visitor.ValidationErrors.First().GetType());
        }

        [TestMethod]
        public void CheckRange_MonthRangesAreProper_ShouldNotHaveExceptions()
        {
            var visitor = "* * * * 1-3,JAN-MAR,MAY-12 * *".TakeVisitor();
            Assert.AreEqual(true, visitor.IsValid);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CheckRange_MonthsRangesAreInproper_ShouldAggregateExceptions()
        {
            var visitor = "* * * * 2-15,MON-TUE,JAN-14 * *".TakeVisitor();
            Assert.AreEqual(false, visitor.IsValid);
            Assert.AreEqual(3, visitor.ValidationErrors.Count());

            Assert.AreEqual(typeof(RangeNodeException), visitor.ValidationErrors.ElementAt(0).GetType());
            Assert.AreEqual(typeof(RangeNodeException), visitor.ValidationErrors.ElementAt(1).GetType());
            Assert.AreEqual(typeof(RangeNodeException), visitor.ValidationErrors.ElementAt(2).GetType());
        }

        [TestMethod]
        public void CheckRange_RangeInDaysOfMonth_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * * 1-5 * *");
        }

        [TestMethod]
        public void CheckRange_RangeInDaysOfWeek_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * * * * 1-5 *");
        }

        [TestMethod]
        public void CheckRange_RangeInHours_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * 1-5 * * *");
        }

        [TestMethod]
        public void CheckRange_RangeInMinutes_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* 1-5 * * * *");
        }

        [TestMethod]
        public void CheckRange_RangeInMonths_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * * * 1-5 * *");
        }
        [TestMethod]
        public void CheckRange_RangeInSeconds_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("1-5 * * * * *");
        }

        [TestMethod]
        public void CheckRange_RangeInYears_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * * * * * 2000-2005");
        }
    }
}
