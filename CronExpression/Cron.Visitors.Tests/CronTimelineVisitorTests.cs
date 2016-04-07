using Cron.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Visitors.Tests
{
    [TestClass]
    public class CronTimelineVisitorTests
    {
        [TestMethod]
        public void ValidateExpression_AllStars_ShouldNotContainErrors()
        {
            var visitor = CreateExpressionAndPerformVisitor("* * * * * * *");

            Assert.AreNotEqual(null, visitor.ValidationErrors);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CronExpresion3()
        {
            var visitor = CreateExpressionAndPerformVisitor("0 0 12 * * ?");

            Assert.AreNotEqual(null, visitor.ValidationErrors);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CronExpression4()
        {
            var visitor = CreateExpressionAndPerformVisitor("0 15 10 ? * *");

            Assert.AreNotEqual(null, visitor.ValidationErrors);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CronExpression5()
        {
            var visitor = CreateExpressionAndPerformVisitor("0 15 10 * * ?");

            Assert.AreNotEqual(null, visitor.ValidationErrors);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CronExpression6()
        {
            var visitor = CreateExpressionAndPerformVisitor("0 15 10 * * ? 2005");

            Assert.AreNotEqual(null, visitor.ValidationErrors);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CronExpression7()
        {
            var visitor = CreateExpressionAndPerformVisitor("0 0-5 14 * * *");

            Assert.AreNotEqual(null, visitor.ValidationErrors);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CronExpression8()
        {
            var visitor = CreateExpressionAndPerformVisitor("0 10,44 14 ? 3 WED");

            Assert.AreNotEqual(null, visitor.ValidationErrors);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CronExpression9()
        {
            var visitor = CreateExpressionAndPerformVisitor("0 15 10 ? * MON-FRI");

            Assert.AreNotEqual(null, visitor.ValidationErrors);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CronExpression10()
        {
            var visitor = CreateExpressionAndPerformVisitor("0 15 10 15 * ?");

            Assert.AreNotEqual(null, visitor.ValidationErrors);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CronExpression11()
        {
            var visitor = CreateExpressionAndPerformVisitor("0 15 10 L * ?");

            Assert.AreNotEqual(null, visitor.ValidationErrors);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CronExpression12()
        {
            var visitor = CreateExpressionAndPerformVisitor("0 15 10 ? * 6L");

            Assert.AreNotEqual(null, visitor.ValidationErrors);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CronExpression13()
        {
            var visitor = CreateExpressionAndPerformVisitor("0 15 10 ? * 6L 2002-2005");

            Assert.AreNotEqual(null, visitor.ValidationErrors);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CronExpression14()
        {
            var visitor = CreateExpressionAndPerformVisitor("0 15 10 ? * 6#3");

            Assert.AreNotEqual(null, visitor.ValidationErrors);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CronExpression15()
        {
            var visitor = CreateExpressionAndPerformVisitor("0 0 12 1/5 * *");

            Assert.AreNotEqual(null, visitor.ValidationErrors);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CronExpression16()
        {
            var visitor = CreateExpressionAndPerformVisitor("0/5 14,18,3-39,52 * ? JAN,MAR,SEP MON-FRI 2002-2016");

            Assert.AreNotEqual(null, visitor.ValidationErrors);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CronExpression17()
        {
            var visitor = CreateExpressionAndPerformVisitor("* 12 10-16/2 * * *");

            Assert.AreNotEqual(null, visitor.ValidationErrors);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        [TestMethod]
        public void CronExpression18()
        {
            var visitor = CreateExpressionAndPerformVisitor("* 12 1-15,17,20-23 * * *");

            Assert.AreNotEqual(null, visitor.ValidationErrors);
            Assert.AreEqual(0, visitor.ValidationErrors.Count());
        }

        private static CronRulesNodeVisitor CreateExpressionAndPerformVisitor(string expression)
        {
            var lexer = new Lexer(expression);
            var parser = new CronParser(lexer);
            var visitor = new CronRulesNodeVisitor();

            var node = parser.ComposeRootComponents();
            node.Accept(visitor);

            return visitor;
        }
    }
}
