using Cron.Visitors.Exceptions;
using Cron.Visitors.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Visitors.Tests
{
    [TestClass]
    public class SpecialOptionsCronRulesTests
    {
        [TestMethod]
        public void SpecialOptions_CheckLInDayInMonths_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * * L * * *");
        }

        [TestMethod]
        public void SpecialOptions_CheckNumericPreceededLInDayInMonths_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * * 2L * * *");
        }

        [TestMethod]
        public void SpecialOptions_CheckNumericPreceededLWithOutOfRangeValue_ShouldThrow()
        {
            var visitor = "* * * * * 1000L *".TakeVisitor();
            Assert.AreEqual(false, visitor.IsValid);
            Assert.AreEqual(1, visitor.ValidationErrors.Count());
            Assert.AreEqual(typeof(UnsupportedValueException), visitor.ValidationErrors.First().GetType());
        }

        [TestMethod]
        public void SpecialOptions_CheckLInIncorrectPlace_ShouldThrow()
        {
            var visitor = "L * * * * * *".TakeVisitor();
            Assert.AreEqual(false, visitor.IsValid);
            Assert.AreEqual(1, visitor.ValidationErrors.Count());
            Assert.AreEqual(typeof(UnexpectedLNodeAtSegment), visitor.ValidationErrors.First().GetType());
        }

        [TestMethod]
        public void SpecialOptions_CheckWInDayInMonths_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * * W * * *");
        }

        [TestMethod]
        public void SpecialOptions_CheckWInIncorrectPlace_ShouldThrow()
        {
            var visitor = "W * * * * * *".TakeVisitor();
            Assert.AreEqual(false, visitor.IsValid);
            Assert.AreEqual(1, visitor.ValidationErrors.Count());
            Assert.AreEqual(typeof(UnexpectedWNodeAtSegment), visitor.ValidationErrors.First().GetType());
        }

        [TestMethod]
        public void SpecialOptions_CheckHashInDayOfWeek_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * * * * 1#2 *");
        }

        [TestMethod]
        public void SpecialOptions_CheckHashNodeLeftValueIsOutOfRangeValues_ShouldThrow()
        {
            var visitor = "* * * * * 100#2 *".TakeVisitor();
            Assert.AreEqual(false, visitor.IsValid);
            Assert.AreEqual(1, visitor.ValidationErrors.Count());
            Assert.AreEqual(typeof(UnsupportedValueException), visitor.ValidationErrors.First().GetType());
        }

        [TestMethod]
        public void SpecialOptions_CheckHashNodeRightValueIsOutOfRangeValues_ShouldThrow()
        {
            var visitor = "* * * * * 1#200 *".TakeVisitor();
            Assert.AreEqual(false, visitor.IsValid);
            Assert.AreEqual(1, visitor.ValidationErrors.Count());
            Assert.AreEqual(typeof(UnsupportedValueException), visitor.ValidationErrors.First().GetType());
        }

        [TestMethod]
        public void SpecialOptions_CheckHashInIncorrectPlace_ShouldThrow()
        {
            var visitor = "1#2 * * * * * *".TakeVisitor();
            Assert.AreEqual(false, visitor.IsValid);
            Assert.AreEqual(1, visitor.ValidationErrors.Count());
            Assert.AreEqual(typeof(UnexpectedHashNodeAtSegment), visitor.ValidationErrors.First().GetType());
        }

        [TestMethod]
        public void SpecialOptions_WNodeIncorectlyWithOtherValuesInSegment_ShouldThrow()
        {
            var visitor = "* * * 1,W,1-5 * *".TakeVisitor();
            Assert.AreEqual(false, visitor.IsValid);
            Assert.AreEqual(1, visitor.ValidationErrors.Count());
            Assert.AreEqual(typeof(WNodeCannotBeMixedException), visitor.ValidationErrors.First().GetType());
        }
    }
}
