using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cron.Visitors.Exceptions;
using Cron.Parser.Helpers;
using Cron.Visitors.Tests.Helpers;
using System.Linq;

namespace Cron.Visitors.Tests
{
    [TestClass]
    public class RangeUnwindCronRulesTests
    {
        public void CheckRange_RangesAreInverted_ShouldThrow()
        {
            var visitor = "* * * * * *".TakeVisitor();
            Assert.AreEqual(false, visitor.IsValid);
            Assert.AreEqual(1, visitor.ValidationErrors.Count());
            Assert.AreEqual(typeof(InvertedRangeException), visitor.ValidationErrors.First().GetType());
        }
    }
}
