using Cron.Visitors;
using Cron.Visitors.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Cron.Parser.Helpers;

namespace Cron.Parser.Tests
{
    [TestClass]
    public class CronRulesTests
    {
        [TestMethod]
        public void CheckRange_RangesSwaped_ShouldReportError()
        {
            CheckErrors("5-1 * * * * * *", false, 1, SemanticErrorKind.SwappedValue);
            CheckErrors("1-4-1 * * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("1-200-1 * * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("150-12 * * * * * *", false, 2, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.SwappedValue);

            CheckErrors("* 5-1 * * * * *", false, 1, SemanticErrorKind.SwappedValue);
            CheckErrors("* 1-4-1 * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* 1-200-1 * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* 150-12 * * * * *", false, 2, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.SwappedValue);

            CheckErrors("* * 5-1 * * * *", false, 1, SemanticErrorKind.SwappedValue);
            CheckErrors("* * 1-4-1 * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * 1-200-1 * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * 150-12 * * * *", false, 2, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.SwappedValue);

            CheckErrors("* * * 5-1 * * *", false, 1, SemanticErrorKind.SwappedValue);
            CheckErrors("* * * 1-4-1 * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * 1-200-1 * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * 150-12 * * *", false, 2, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.SwappedValue);

            CheckErrors("* * * * MAR-JAN * *", false, 1, SemanticErrorKind.SwappedValue);
            CheckErrors("* * * * 5-1 * *", false, 1, SemanticErrorKind.SwappedValue);
            CheckErrors("* * * * 1-4-1 * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * 1-200-1 * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * MON-TUE,JAN-14 * *", false, 3, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.ValueOutOfRange);

            //Here behaviour is different. Regulary it should return ValueOutOfRange and SwappedValue errors.
            //However, due to it's in month/dayofweek field, 150 is uncomparable to 12. That won't trigger SwappedValue error.
            //Can't tell that "150" is swapped with "12" becouse it's uncompareable.
            CheckErrors("* * * * 150-12 * *", false, 1, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * * * 15-5 *", false, 1, SemanticErrorKind.ValueOutOfRange);

            CheckErrors("* * * * * FRI-MON *", false, 1, SemanticErrorKind.SwappedValue);
            CheckErrors("* * * * * 5-1 *", false, 1, SemanticErrorKind.SwappedValue);
            CheckErrors("* * * * * 1-4-1 *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * * 1-200-1 *", false, 1, SemanticErrorKind.UnsupportedValue);

            CheckErrors("* * * * * * 2010-2000", false, 1, SemanticErrorKind.SwappedValue);
            CheckErrors("* * * * * * 2000-2010-2000", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * * * 2000-2010-2000", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * * * 3500-2000", false, 2, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.SwappedValue);
        }

        [TestMethod]
        public void CheckRange_RangesExceed_ShouldReportError()
        {
            CheckErrors("150-200 * * * * * *", false, 2, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* 150-200 * * * * *", false, 2, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * 150-200 * * * *", false, 2, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * 150-200 * * *", false, 2, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * * 150-200 * *", false, 2, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * * * 150-200 *", false, 2, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * * * * 150-200", false, 2, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.ValueOutOfRange);
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
            CheckErrors("JAN * * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* JAN * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * JAN * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * JAN * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * BLE * *", false, 1, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * * * BLE *", false, 1, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * * * * JAN", false, 1, SemanticErrorKind.UnsupportedValue);
        }

        [TestMethod]
        public void CheckQuestionMark_ShouldReportErrorWhenUnsupported()
        {
            CheckErrors("? * * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* ? * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * ? * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * ? * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * * * ? * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * * ? *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * * * * * ?", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * ?,1 * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * * ?,1 *", false, 1, SemanticErrorKind.UnsupportedValue);
        }

        [TestMethod]
        public void CheckLNode_ShouldReportError()
        {
            CheckErrors("L * * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* L * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * L * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * L * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * * * L", false, 1, SemanticErrorKind.UnsupportedValue);
        }

        [TestMethod]
        public void CheckLNode_ShouldPass()
        {
            CheckErrors("* * * L * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * * * * L *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
        }

        [TestMethod]
        public void CheckNumericPrecededLNode_ShouldReportError()
        {
            CheckErrors("1L * * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* 1L * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * 1L * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * 1L * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * * * 1L", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * * 1000L *", false, 1, SemanticErrorKind.ValueOutOfRange);
        }

        [TestMethod]
        public void CheckNumericPrecededLNode_ShouldPass()
        {
            CheckErrors("* * * 1L * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * * * * 1L *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
        }

        [TestMethod]
        public void CheckNumericPrecededWNode_ShouldReportError()
        {
            CheckErrors("1W * * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* 1W * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * 1W * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * 1W * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * * 1W *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * * * 1W", false, 1, SemanticErrorKind.UnsupportedValue);
        }

        [TestMethod]
        public void CheckNumericPrecededWNode_ShouldPass()
        {
            CheckErrors("* * * 1W * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
        }

        [TestMethod]
        public void CheckHashNode_ShouldReportError()
        {
            CheckErrors("1#5 * * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* 1#5 * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * 1#5 * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * 1#5 * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * 1#5 * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * * * 1#5", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * * 0#5 *", false, 2, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.ValueOutOfRange);
        }

        [TestMethod]
        public void CheckHashNode_ShouldPass()
        {
            CheckErrors("* * * * * 1#4 *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
        }

        [TestMethod]
        public void CheckEndOfFile_ShouldReportError()
        {
            CheckErrors("* * *", false, 1, SyntaxErrorKind.MissingValue);
            CheckErrors("* * * * *", false, 1, SyntaxErrorKind.MissingValue);

            //check won't throw exception when short expression without seconds and years passed.
            //Should pass becouse parser will prepend and append missing seconds, year values. Used for old way
            //provided expressions when such segments doesn't exists.
            CheckErrors<SemanticErrorKind>(() => {
                var nodes = "* * * * *".Parse(true, true, true);
                var visitor = new CronRulesNodeVisitor(true);
                nodes.Accept(visitor);
                return visitor;
            }, (error, type) => ((SemanticError)error).Kind == type, true, 0, ArrayHelper.Empty<SemanticErrorKind>());
        }

        [TestMethod]
        public void CheckWNode_ShouldReportError()
        {
            CheckErrors("W * * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* W * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * W * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * W * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * * W *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * * * W", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * * * 1,W", false, 2, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * 1,W,1-5 * *", false, 1, SemanticErrorKind.UnsupportedValue);
        }

        [TestMethod]
        public void CheckWNode_ShouldPass()
        {
            CheckErrors("* * * W * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
        }

        [TestMethod]
        public void CheckLWNode_ShouldPass()
        {
            CheckErrors("* * * LW * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
        }

        [TestMethod]
        public void CheckLWNode_ShouldReportError()
        {
            CheckErrors("LW * * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* LW * * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * LW * * * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * LW * *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * * LW *", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("* * * * * * LW", false, 1, SemanticErrorKind.UnsupportedValue);
        }

        [TestMethod]
        public void CheckNumberNode_ShouldPass()
        {
            CheckErrors("1 * * * * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* 2 * * * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * 3 * * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * * 4 * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * * * 5 * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * * * * 6 *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * * * * * 2000", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
        }

        [TestMethod]
        public void CheckNumberNode_ShouldReportError()
        {
            CheckErrors("60 * * * * * *", false, 1, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* 60 * * * * *", false, 1, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * 24 * * * *", false, 1, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * 32 * * *", false, 1, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * * 15 * *", false, 1, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * * * 8 *", false, 1, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * * * * 3001", false, 1, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * * * * 1000", false, 1, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * * * * 1000,3001", false, 2, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.ValueOutOfRange);
        }

        [TestMethod]
        public void CheckIncrementByNode_ShouldReportError()
        {
            CheckErrors("1-2/60 * * * * * *", false, 1, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("2-1/60 * * * * * *", false, 2, SemanticErrorKind.ValueOutOfRange, SemanticErrorKind.SwappedValue);
            CheckErrors("* 1-2/60 * * * * *", false, 1, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * 1-2/24 * * * *", false, 1, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * 1-2/32 * * *", false, 1, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * * 1-2/15 * *", false, 1, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * * * 1-2/8 *", false, 1, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * * * * 2-1/3001", false, 3,
                SemanticErrorKind.ValueOutOfRange,
                SemanticErrorKind.ValueOutOfRange,
                SemanticErrorKind.SwappedValue);
            CheckErrors("* * * * * * 2000-2010/0", false, 1, SemanticErrorKind.ValueOutOfRange);
        }

        [TestMethod]
        public void CheckIncrementByNode_ShouldPass()
        {
            CheckErrors("1-2/4 * * * * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* 1-2/4 * * * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * 1-2/4 * * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * * 1-2/4 * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * * * 1-2/4 * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * * * * 1-2/4 *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * * * * * 2000-2010/4", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("1/5 * * * * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* 1/5 * * * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * 1/5 * * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * * 1/5 * * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * * * 1/5 * *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * * * * 1/5 *", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
            CheckErrors("* * * * * * 2000/5", true, 0, ArrayHelper.Empty<SemanticErrorKind>());
        }

        [TestMethod]
        public void CheckStarNode_ShouldReportError()
        {
            CheckErrors("*,1 1 1 1 1 1 2000", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("1 1,* 1 1 1 1 2000", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("1 1 2-3,* 1 1 1 2000", false, 1, SemanticErrorKind.UnsupportedValue);
            CheckErrors("1 1 1 *,1#4 1 1 2000", false, 2, SemanticErrorKind.UnsupportedValue, SemanticErrorKind.UnsupportedValue);
            CheckErrors("1 1 1 1 *, 1 2000", false, 2, SemanticErrorKind.UnsupportedValue, SyntaxErrorKind.MissingValue);
            CheckErrors("1 1 1 1 1 *#5 2000", false, 2, SemanticErrorKind.UnsupportedValue, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("1 1 1 1 1 *#* 2000", false, 2, SemanticErrorKind.UnsupportedValue, SemanticErrorKind.UnsupportedValue);
            CheckErrors("1 1 1 1 1 *## 2000", false, 1, SemanticErrorKind.UnsupportedValue);
        }

        [TestMethod]
        public void CheckCommaNode_ShouldReportError()
        {
            CheckErrors("1, * * * * * *", false, 1, SyntaxErrorKind.MissingValue);
            CheckErrors("* 1, * * * * *", false, 1, SyntaxErrorKind.MissingValue);
            CheckErrors("* * ,1 * * * *", false, 1, SyntaxErrorKind.MissingValue);
            CheckErrors("* * * 1,1, * * *", false, 1, SyntaxErrorKind.MissingValue);
            CheckErrors("* * * * ,1,1 * *", false, 1, SyntaxErrorKind.MissingValue);
            CheckErrors("* * * * ,1,2000,1 * *", false, 2, SyntaxErrorKind.MissingValue, SemanticErrorKind.ValueOutOfRange);
            CheckErrors("* * * * * ,1,2, *", false, 2, SyntaxErrorKind.MissingValue, SyntaxErrorKind.MissingValue);
            CheckErrors("* * * * * * ,", false, 2, SyntaxErrorKind.MissingValue, SyntaxErrorKind.MissingValue);
        }

        public static void CheckErrors(string expression, bool shouldBeValid, int expectedCountOfErrors, params SyntaxErrorKind[] types)
        {
            CheckErrors(
                () => expression.TakeVisitor(),
                (error, expectedKind) => ((SyntaxError)error).Kind == expectedKind,
                shouldBeValid,
                expectedCountOfErrors,
                types);
        }

        public static void CheckErrors(string expression, bool shouldBeValid, int expectedCountOfErrors, params object[] types)
        {
            CheckErrors(
                () => expression.TakeVisitor(),
                (error, expectedKind) =>
                {
                    switch (expectedKind.GetType().Name)
                    {
                        case nameof(SyntaxErrorKind):
                            return error is SyntaxError && ((SyntaxError)error).Kind == (SyntaxErrorKind)expectedKind;
                        case nameof(SemanticErrorKind):
                            return error is SemanticError && ((SemanticError)error).Kind == (SemanticErrorKind)expectedKind;
                    }
                    return false;
                }, shouldBeValid, expectedCountOfErrors, types);
        }

        public static void CheckErrors(string expression, bool shouldBeValid, int expectedCountOfErrors, params SemanticErrorKind[] types)
        {
            CheckErrors(
                () => expression.TakeVisitor(),
                (error, expectedKind) => ((SemanticError)error).Kind == expectedKind,
                shouldBeValid,
                expectedCountOfErrors,
                types);
        }

        public static void CheckErrors<TErrorKind>(Func<CronRulesNodeVisitor> createFunc, Func<VisitationMessage, TErrorKind, bool> compareFunc, bool shouldBeValid, int expectedCountOfErrors, params TErrorKind[] types)
        {
            Assert.IsNotNull(compareFunc);
            Assert.IsNotNull(createFunc);
            var visitor = createFunc?.Invoke();
            Assert.AreEqual(shouldBeValid, visitor.IsValid);
            Assert.AreEqual(expectedCountOfErrors, visitor.Errors.Count());
            Assert.AreEqual(expectedCountOfErrors, types.Count());
            var errors = visitor.Errors;
            foreach (var type in types)
            {
                var firstOfType = errors.First(f => compareFunc(f, type));
                errors = errors.Where(f => !ReferenceEquals(firstOfType, f));
            }
        }

        public static void CheckRange_CheckForRangesExceed_ShouldReportError(string expression)
        {
            var visitor = expression.TakeVisitor();
            Assert.IsFalse(visitor.IsValid);
            Assert.AreEqual(2, visitor.Errors.Count());
            Assert.AreEqual(SemanticErrorKind.ValueOutOfRange, visitor.Errors.OfType<SyntaxError>().First().Kind);
            Assert.AreEqual(SemanticErrorKind.ValueOutOfRange, visitor.Errors.OfType<SyntaxError>().ElementAt(1).Kind);
        }

        [TestMethod]
        public void CheckRange_MinutesRangesSwaped_ShouldReportError()
        {
            CheckRange_ShouldReportError("* 5-1 * * * * *", 1, SemanticErrorKind.SwappedValue);
            CheckRange_ShouldReportError("* 1-4-1 * * * * *", 1, SemanticErrorKind.UnsupportedValue);
            CheckRange_ShouldReportError("* 1-200-1 * * * * *", 1, SemanticErrorKind.UnsupportedValue);
        }

        [TestMethod]
        public void CheckRange_MinutesRangesExceed_ShouldReportError()
        {
            var visitor = "* 150-200 * * * * *".TakeVisitor();
            Assert.IsFalse(visitor.IsValid);
            Assert.AreEqual(2, visitor.Errors.Count());
            Assert.AreEqual(SemanticErrorKind.ValueOutOfRange, visitor.Errors.OfType<SemanticError>().First().Kind);
            Assert.AreEqual(SemanticErrorKind.ValueOutOfRange, visitor.Errors.OfType<SemanticError>().ElementAt(1).Kind);
        }

        private static void CheckRange_ShouldReportError(string expression, int expectedCount, SemanticErrorKind expectedError)
        {
            var visitor = expression.TakeVisitor();
            Assert.IsFalse(visitor.IsValid);
            Assert.AreEqual(expectedCount, visitor.Errors.OfType<SemanticError>().Count());
            Assert.AreEqual(expectedError, visitor.Errors.OfType<SemanticError>().First().Kind);
        }

        [TestMethod]
        public void CheckRange_AllRangesAreIncorrect_ShouldAggregateExceptions()
        {
            CheckErrors(
                "1-100 60-120 24-48 60-45 13-23 8-15 0-5",
                false,
                14,
                SemanticErrorKind.ValueOutOfRange,
                SemanticErrorKind.ValueOutOfRange,
                SemanticErrorKind.ValueOutOfRange,
                SemanticErrorKind.ValueOutOfRange,
                SemanticErrorKind.ValueOutOfRange,
                SemanticErrorKind.ValueOutOfRange,
                SemanticErrorKind.ValueOutOfRange,
                SemanticErrorKind.ValueOutOfRange,
                SemanticErrorKind.ValueOutOfRange,
                SemanticErrorKind.ValueOutOfRange,
                SemanticErrorKind.ValueOutOfRange,
                SemanticErrorKind.ValueOutOfRange,
                SemanticErrorKind.ValueOutOfRange,
                SemanticErrorKind.SwappedValue);
        }

        [TestMethod]
        public void CheckRange_MonthRangesAreProper_ShouldNotHaveExceptions()
        {
            var visitor = "* * * * 1-3,JAN-MAR,MAY-12 * *".TakeVisitor();
            Assert.AreEqual(true, visitor.IsValid);
            Assert.AreEqual(0, visitor.Errors.Count());
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
