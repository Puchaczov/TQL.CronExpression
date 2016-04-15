using Cron.Visitors.Exceptions;
using Cron.Visitors.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cron.Visitors.Helpers;
using System.Linq;

namespace Cron.Visitors.Tests
{
    [TestClass]
    public class SimpleCronRulesTests
    {

        [TestMethod]
        public void CheckValues_AllSimpleValuesAreCorrect_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("0 0 0 1 1 1 2000");
        }
        [TestMethod]
        public void CheckValues_AllValuesAreOutOfRange_ShouldAggregateExceptions()
        {
            var visitor = "90 90 90 133,AXD 15,FFF 8 0".TakeVisitor();
            Assert.AreEqual(false, visitor.IsValid);
            Assert.AreEqual(9, visitor.ValidationErrors.Count());
            Assert.AreEqual(true, visitor.ValidationErrors.OfType<UnexpectedWordNodeAtSegment>().Any());
        }

        [TestMethod]
        public void CheckValues_DayOfWeeksAreCommaSeperated_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * * * * 1,2,WED,THU *");
        }

        [TestMethod]
        public void CheckValues_DaysInMonthAreCommaSeperated_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * * 1,2,5,8 * * *");
        }

        [TestMethod]
        public void CheckValues_HoursAreCommaSeparated_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * 1,2,5,8 * * * *");
        }

        [TestMethod]
        public void CheckValues_MinutesAreCommaSeparated_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* 1,2,5,8 * * * * *");
        }

        [TestMethod]
        public void CheckValues_MonthsAreCommaSeperated_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * * * 1,FEB,3,NOV,DEC * *");
        }

        [TestMethod]
        public void CheckValues_SecondsAreCommaSeparated_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("1,2,5,8 * * * * * *");
        }

        [TestMethod]
        public void CheckValues_YearsAreCommaSeperated_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * * * * * 2000,2001");
        }
    }
}
