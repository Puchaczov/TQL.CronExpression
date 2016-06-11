using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TQL.CronExpression.Parser.Helpers;

namespace TQL.CronExpression.Visitors.Tests.Helpers
{
    internal static class TestsHelper
    {

        public static void CheckExpressionDidNotReturnsValidationErrors(string expression)
        {
            var visitor = expression.TakeVisitor();
            Assert.AreEqual(true, visitor.IsValid);
            Assert.AreEqual(0, visitor.Errors.Count());
        }

        public static CronRulesNodeVisitor TakeVisitor(this string expression, bool produceMissingYearComponent = true, bool produceEndOfFileNodeComponent = true, bool produceMissingSecondComponent = false)
        {
            var nodes = expression.Parse(produceMissingYearComponent, produceEndOfFileNodeComponent, produceMissingSecondComponent);
            var visitor = new CronRulesNodeVisitor();
            nodes.Accept(visitor);
            return visitor;
        }
    }
}
