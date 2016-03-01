using Cron.Parser.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Cron.Visitors.Tests.Helpers
{
    internal static class TestsHelper
    {
        public static CronRulesNodeVisitor TakeVisitor(this string expression)
        {
            var nodes = expression.Parse();
            var visitor = new CronRulesNodeVisitor();
            nodes.Accept(visitor);
            return visitor;
        }

        public static void CheckExpressionDidNotReturnsValidationErrors(string expression)
        {
            var visitor = expression.TakeVisitor();
            Assert.AreEqual(true, visitor.IsValid);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }
    }
}
