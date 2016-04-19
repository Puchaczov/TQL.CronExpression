using Cron.Visitors;
using Cron.Visitors.Exceptions;
using Cron.Visitors.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Cron.Parser.Tests
{
    [TestClass]
    public class RangeCronRulesTests
    {
        [TestMethod]
        public void CheckRange_RangesSwaped_ShouldReportError()
        {
            CheckRange_ShouldReportError("5-1 * * * * * *", 1, SyntaxErrorKind.SwappedValue);
            CheckRange_ShouldReportError("1-4-1 * * * * * *", 1, SyntaxErrorKind.UnsupportedValue);
            CheckRange_ShouldReportError("1-200-1 * * * * * *", 1, SyntaxErrorKind.UnsupportedValue);

            CheckRange_ShouldReportError("* 5-1 * * * * *", 1, SyntaxErrorKind.SwappedValue);
            CheckRange_ShouldReportError("* 1-4-1 * * * * *", 1, SyntaxErrorKind.UnsupportedValue);
            CheckRange_ShouldReportError("* 1-200-1 * * * * *", 1, SyntaxErrorKind.UnsupportedValue);

            CheckRange_ShouldReportError("* * 5-1 * * * *", 1, SyntaxErrorKind.SwappedValue);
            CheckRange_ShouldReportError("* * 1-4-1 * * * *", 1, SyntaxErrorKind.UnsupportedValue);
            CheckRange_ShouldReportError("* * 1-200-1 * * * *", 1, SyntaxErrorKind.UnsupportedValue);

            CheckRange_ShouldReportError("* * * 5-1 * * *", 1, SyntaxErrorKind.SwappedValue);
            CheckRange_ShouldReportError("* * * 1-4-1 * * *", 1, SyntaxErrorKind.UnsupportedValue);
            CheckRange_ShouldReportError("* * * 1-200-1 * * *", 1, SyntaxErrorKind.UnsupportedValue);

            CheckRange_ShouldReportError("* * * * MAR-JAN * *", 1, SyntaxErrorKind.SwappedValue);
            CheckRange_ShouldReportError("* * * * 5-1 * *", 1, SyntaxErrorKind.SwappedValue);
            CheckRange_ShouldReportError("* * * * 1-4-1 * *", 1, SyntaxErrorKind.UnsupportedValue);
            CheckRange_ShouldReportError("* * * * 1-200-1 * *", 1, SyntaxErrorKind.UnsupportedValue);

            CheckRange_ShouldReportError("* * * * * FRI-MON *", 1, SyntaxErrorKind.SwappedValue);
            CheckRange_ShouldReportError("* * * * * 5-1 *", 1, SyntaxErrorKind.SwappedValue);
            CheckRange_ShouldReportError("* * * * * 1-4-1 *", 1, SyntaxErrorKind.UnsupportedValue);
            CheckRange_ShouldReportError("* * * * * 1-200-1 *", 1, SyntaxErrorKind.UnsupportedValue);

            CheckRange_ShouldReportError("* * * * * * 2010-2000", 1, SyntaxErrorKind.SwappedValue);
            CheckRange_ShouldReportError("* * * * * * 2000-2010-2000", 1, SyntaxErrorKind.UnsupportedValue);
            CheckRange_ShouldReportError("* * * * * * 2000-2010-2000", 1, SyntaxErrorKind.UnsupportedValue);
        }

        [TestMethod]
        public void CheckRange_RangesExceed_ShouldReportError()
        {
            CheckRange_CheckForRangesExceed_ShouldReportError("150-200 * * * * * *");
            CheckRange_CheckForRangesExceed_ShouldReportError("* 150-200 * * * * *");
            CheckRange_CheckForRangesExceed_ShouldReportError("* * 150-200 * * * *");
            CheckRange_CheckForRangesExceed_ShouldReportError("* * * 150-200 * * *");
            CheckRange_CheckForRangesExceed_ShouldReportError("* * * * 150-200 * *");
            CheckRange_CheckForRangesExceed_ShouldReportError("* * * * * 150-200 *");
            CheckRange_CheckForRangesExceed_ShouldReportError("* * * * * * 150-200");
        }

        [TestMethod]
        public void CheckRange_RangesWithMissingNodes_ShouldReportError()
        {
            CheckErrors("- * * * * * *", false, 2, SyntaxErrorKind.MissingValue, SyntaxErrorKind.MissingValue);
            CheckErrors("1- * * * * * *", false, 1, SyntaxErrorKind.MissingValue);
            CheckErrors("-1 * * * * * *", false, 1, SyntaxErrorKind.MissingValue);

            CheckErrors("* - * * * * *", false, 2, SyntaxErrorKind.MissingValue, SyntaxErrorKind.MissingValue);
            CheckErrors("* 1- * * * * *", false, 1, SyntaxErrorKind.MissingValue);
            CheckErrors("* -1 * * * * *", false, 1, SyntaxErrorKind.MissingValue);

            CheckErrors("* * - * * * *", false, 2, SyntaxErrorKind.MissingValue, SyntaxErrorKind.MissingValue);
            CheckErrors("* * 1- * * * *", false, 1, SyntaxErrorKind.MissingValue);
            CheckErrors("* * -1 * * * *", false, 1, SyntaxErrorKind.MissingValue);

            CheckErrors("* * * - * * *", false, 2, SyntaxErrorKind.MissingValue, SyntaxErrorKind.MissingValue);
            CheckErrors("* * * 1- * * *", false, 1, SyntaxErrorKind.MissingValue);
            CheckErrors("* * * -1 * * *", false, 1, SyntaxErrorKind.MissingValue);

            CheckErrors("* * * * - * *", false, 2, SyntaxErrorKind.MissingValue, SyntaxErrorKind.MissingValue);
            CheckErrors("* * * * 1- * *", false, 1, SyntaxErrorKind.MissingValue);
            CheckErrors("* * * * -1 * *", false, 1, SyntaxErrorKind.MissingValue);

            CheckErrors("* * * * * - *", false, 2, SyntaxErrorKind.MissingValue, SyntaxErrorKind.MissingValue);
            CheckErrors("* * * * * 1- *", false, 1, SyntaxErrorKind.MissingValue);
            CheckErrors("* * * * * -1 *", false, 1, SyntaxErrorKind.MissingValue);

            CheckErrors("* * * * * * -", false, 2, SyntaxErrorKind.MissingValue, SyntaxErrorKind.MissingValue);
            CheckErrors("* * * * * * 2000-", false, 1, SyntaxErrorKind.MissingValue);
            CheckErrors("* * * * * * -2000", false, 1, SyntaxErrorKind.MissingValue);
        }

        [TestMethod]
        public void CheckWord_ShouldReportErrors()
        {
            CheckErrors("JAN * * * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* JAN * * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * JAN * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * * JAN * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * * * BLE * *", false, 1, SyntaxErrorKind.ValueOutOfRange);
            CheckErrors("* * * * * BLE *", false, 1, SyntaxErrorKind.ValueOutOfRange);
            CheckErrors("* * * * * * JAN", false, 1, SyntaxErrorKind.UnsupportedValue);
        }

        [TestMethod]
        public void CheckQuestionMark_ShouldReportErrorWhenUnsupported()
        {
            CheckErrors("? * * * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* ? * * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * ? * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * * ? * * *", true, 0);
            CheckErrors("* * * * ? * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * * * * ? *", true, 0);
            CheckErrors("* * * * * * ?", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * * * ?,1 * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * * * * ?,1 *", false, 1, SyntaxErrorKind.UnsupportedValue);
        }

        [TestMethod]
        public void CheckLNode_ShouldReportError()
        {
            CheckErrors("L * * * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* L * * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * L * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * * * L * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * * * * * L", false, 1, SyntaxErrorKind.UnsupportedValue);
        }

        [TestMethod]
        public void CheckLNode_ShouldPass()
        {
            CheckErrors("* * * L * * *", true, 0);
            CheckErrors("* * * * * L *", true, 0);
        }

        [TestMethod]
        public void CheckNumericPrecededLNode_ShouldReportError()
        {
            CheckErrors("1L * * * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* 1L * * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * 1L * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * * * 1L * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * * * * * 1L", false, 1, SyntaxErrorKind.UnsupportedValue);
        }

        [TestMethod]
        public void CheckNumericPrecededLNode_ShouldPass()
        {
            CheckErrors("* * * 1L * * *", true, 0);
            CheckErrors("* * * * * 1L *", true, 0);
        }

        [TestMethod]
        public void CheckNumericPrecededWNode_ShouldReportError()
        {
            CheckErrors("1W * * * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* 1W * * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * 1W * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * * * 1W * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * * * * 1W *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * * * * * 1W", false, 1, SyntaxErrorKind.UnsupportedValue);
        }

        [TestMethod]
        public void CheckNumericPrecededWNode_ShouldPass()
        {
            CheckErrors("* * * 1W * * *", true, 0);
        }

        [TestMethod]
        public void CheckHashNode_ShouldReportError()
        {
            CheckErrors("1#5 * * * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* 1#5 * * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * 1#5 * * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * * 1#5 * * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * * * 1#5 * *", false, 1, SyntaxErrorKind.UnsupportedValue);
            CheckErrors("* * * * * * 1#5", false, 1, SyntaxErrorKind.UnsupportedValue);
        }

        [TestMethod]
        public void CheckHashNode_ShouldPass()
        {
            CheckErrors("* * * * * 1#4 *", true, 0);
        }

        public static void CheckErrors(string expression, bool shouldBeValid, int expectedCountOfErrors, params SyntaxErrorKind[] types)
        {
            var visitor = expression.TakeVisitor();
            Assert.AreEqual(shouldBeValid, visitor.IsValid);
            Assert.AreEqual(expectedCountOfErrors, visitor.SyntaxErrors.Count());
            Assert.AreEqual(expectedCountOfErrors, types.Count());
            var errors = visitor.SyntaxErrors;
            foreach (var type in types)
            {
                errors = errors.Where(f => f != errors.OfType<SyntaxError>().Single(p => p.Kind == type));
            }
        }

        public static void CheckRange_CheckForRangesExceed_ShouldReportError(string expression)
        {
            var visitor = expression.TakeVisitor();
            Assert.IsFalse(visitor.IsValid);
            Assert.AreEqual(2, visitor.SyntaxErrors.Count());
            Assert.AreEqual(SyntaxErrorKind.ValueOutOfRange, visitor.SyntaxErrors.OfType<SyntaxError>().First().Kind);
            Assert.AreEqual(SyntaxErrorKind.ValueOutOfRange, visitor.SyntaxErrors.OfType<SyntaxError>().ElementAt(1).Kind);
        }

        [TestMethod]
        public void CheckRange_SecondsLeftValueExceed_ShouldReportError()
        {
            var visitor = "150-12 * * * * * *".TakeVisitor();
            Assert.IsFalse(visitor.IsValid);
            Assert.AreEqual(1, visitor.SyntaxErrors.Count());
            Assert.AreEqual(SyntaxErrorKind.ValueOutOfRange, visitor.SyntaxErrors.OfType<SyntaxError>().First().Kind);
        }

        [TestMethod]
        public void CheckRange_SecondsUnsupportedRangeValue_ShouldReportError()
        {
            var visitor = "*-5 * * * * * *".TakeVisitor();
            Assert.IsFalse(visitor.IsValid);
            Assert.AreEqual(1, visitor.SyntaxErrors.Count());
            Assert.AreEqual(SyntaxErrorKind.UnsupportedValue, visitor.SyntaxErrors.OfType<SyntaxError>().First().Kind);

            visitor = "*-* * * * * * *".TakeVisitor();
            Assert.IsFalse(visitor.IsValid);
            Assert.AreEqual(2, visitor.SyntaxErrors.Count());
            Assert.AreEqual(SyntaxErrorKind.UnsupportedValue, visitor.SyntaxErrors.OfType<SyntaxError>().First().Kind);
            Assert.AreEqual(SyntaxErrorKind.UnsupportedValue, visitor.SyntaxErrors.OfType<SyntaxError>().First().Kind);
        }

        [TestMethod]
        public void CheckRange_SecondsUnsupportedComplexNodes_ShouldReportError()
        {
            var visitor = "1#5-5 * * * * * *".TakeVisitor();
            Assert.IsFalse(visitor.IsValid);
            Assert.AreEqual(1, visitor.SyntaxErrors.Count());
            Assert.AreEqual(SyntaxErrorKind.UnsupportedValue, visitor.SyntaxErrors.OfType<SyntaxError>().First().Kind);
        }

        [TestMethod]
        public void CheckRange_MinutesRangesSwaped_ShouldReportError()
        {
            CheckRange_ShouldReportError("* 5-1 * * * * *", 1, SyntaxErrorKind.SwappedValue);
            CheckRange_ShouldReportError("* 1-4-1 * * * * *", 1, SyntaxErrorKind.UnsupportedValue);
            CheckRange_ShouldReportError("* 1-200-1 * * * * *", 1, SyntaxErrorKind.UnsupportedValue);
        }

        [TestMethod]
        public void CheckRange_MinutesRangesExceed_ShouldReportError()
        {
            var visitor = "* 150-200 * * * * *".TakeVisitor();
            Assert.IsFalse(visitor.IsValid);
            Assert.AreEqual(2, visitor.SyntaxErrors.Count());
            Assert.AreEqual(SyntaxErrorKind.ValueOutOfRange, visitor.SyntaxErrors.OfType<SyntaxError>().First().Kind);
            Assert.AreEqual(SyntaxErrorKind.ValueOutOfRange, visitor.SyntaxErrors.OfType<SyntaxError>().ElementAt(1).Kind);
        }

        private static void CheckRange_ShouldReportError(string expression, int expectedCount, SyntaxErrorKind expectedError)
        {
            var visitor = expression.TakeVisitor();
            Assert.IsFalse(visitor.IsValid);
            Assert.AreEqual(expectedCount, visitor.SyntaxErrors.OfType<SyntaxError>().Count());
            Assert.AreEqual(expectedError, visitor.SyntaxErrors.OfType<SyntaxError>().First().Kind);
        }

        private static void CheckRange_RangesExceed_ShouldReportError(string expression)
        {
            var visitor = expression.TakeVisitor();
            Assert.IsFalse(visitor.IsValid);
            Assert.AreEqual(2, visitor.SyntaxErrors.Count());
            Assert.AreEqual(SyntaxErrorKind.ValueOutOfRange, visitor.SyntaxErrors.OfType<SyntaxError>().First().Kind);
            Assert.AreEqual(SyntaxErrorKind.ValueOutOfRange, visitor.SyntaxErrors.OfType<SyntaxError>().ElementAt(1).Kind);
        }

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
