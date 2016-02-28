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
    public class SimpleCronRulesTests
    {
        [TestMethod]
        public void CheckValues_AllValuesAreOutOfRange_ShouldAggregateExceptions()
        {
            var visitor = "90 90 90 133,AXD 15,FFF 8 0".TakeVisitor();
            Assert.AreEqual(false, visitor.IsValid);
            Assert.AreEqual(9, visitor.ValidationErrors.Count());
            Assert.AreEqual(true, visitor.ValidationErrors.OfType<UnexpectedWordNodeAtSegment>().Any());
        }

        [TestMethod]
        public void CheckValues_AllSimpleValuesAreCorrect_ShouldPass()
        {
            var visitor = "0 0 0 1 1 1 2000".TakeVisitor();
            Assert.AreEqual(true, visitor.IsValid);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CheckValues_SecondsAreCommaSeparated_ShouldPass()
        {
            var visitor = "1,2,5,8 * * * * * *".TakeVisitor();
            Assert.AreEqual(true, visitor.IsValid);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }
    }
}
