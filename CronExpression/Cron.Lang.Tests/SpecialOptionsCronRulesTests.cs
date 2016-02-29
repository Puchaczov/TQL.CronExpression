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
        public void SpecialOptions_CheckLInIncorrectPlace_ShouldThrow()
        {
            var visitor = "L * * * * * *".TakeVisitor();
            Assert.AreEqual(false, visitor.IsValid);
            Assert.AreEqual(1, visitor.ValidationErrors.Count());
            Assert.AreEqual(typeof(UnexpectedLNodeAtSegment), visitor.ValidationErrors.First().GetType());
        }
    }
}
