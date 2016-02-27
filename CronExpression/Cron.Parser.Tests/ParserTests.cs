using Cron.Parser.Exceptions;
using Cron.Parser.Helpers;
using Cron.Parser.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void CheckSyntaxTree_AllStarNodes_ShouldPass()
        {
            var tree = CheckSyntaxTree("* * * * * * *");
            CheckHasAppropiateCountsOfSegments(tree);
            CheckLastOfSegmentsIsOfType<EndOfFileNode>(tree);
        }


        [TestMethod]
        public void CheckSyntaxTree_WithRangeNode_ShouldPass()
        {
            var tree = CheckSyntaxTree("1-5 * * * * * 2000-3000");
            CheckHasAppropiateCountsOfSegments(tree);
            CheckLastOfSegmentsIsOfType<EndOfFileNode>(tree);
        }

        [TestMethod]
        public void CheckSyntaxTree_WithIncNode_ShouldPass()
        {
            var tree = CheckSyntaxTree("1/5 * * 0-7 * * *");
            CheckHasAppropiateCountsOfSegments(tree);
            CheckLastOfSegmentsIsOfType<EndOfFileNode>(tree);
        }

        [TestMethod]
        public void CheckSyntaxTree_WithCommaNodes_ShouldPass()
        {
            var tree = CheckSyntaxTree("1,2,3,4 7,6,5,4 * * * * *");
            CheckHasAppropiateCountsOfSegments(tree);
            CheckLastOfSegmentsIsOfType<EndOfFileNode>(tree);
        }

        [TestMethod]
        public void CheckSyntaxTree_WithLOption_ShouldPass()
        {
            var tree = CheckSyntaxTree("* L * * L * *");
            CheckHasAppropiateCountsOfSegments(tree);
            CheckLastOfSegmentsIsOfType<EndOfFileNode>(tree);
        }

        [TestMethod]
        public void CheckSyntaxTree_WithWOption_ShouldPass()
        {
            var tree = CheckSyntaxTree("W * * W * * *");
            CheckHasAppropiateCountsOfSegments(tree);
            CheckLastOfSegmentsIsOfType<EndOfFileNode>(tree);
        }

        [TestMethod]
        public void CheckSyntaxTree_WithLWOption_ShouldPass()
        {
            var tree = CheckSyntaxTree("LW LW LW * * * *");
            CheckHasAppropiateCountsOfSegments(tree);
            CheckLastOfSegmentsIsOfType<EndOfFileNode>(tree);
        }

        [TestMethod]
        public void CheckSyntaxTree_WithMonthsRange_ShouldPass()
        {
            var tree = CheckSyntaxTree("MON-WED * SAT-MON * * * *");
            CheckHasAppropiateCountsOfSegments(tree);
            CheckLastOfSegmentsIsOfType<EndOfFileNode>(tree);
        }

        [TestMethod]
        public void CheckSyntaxTree_WithYearUnspecified_ShouldPassWithAppendedYearStarNode()
        {
            var tree = CheckSyntaxTree("* * * * * *", "* * * * * * *");
            CheckHasAppropiateCountsOfSegments(tree);
            CheckLastOfSegmentsIsOfType<EndOfFileNode>(tree);
        }

        [TestMethod]
        public void CheckSyntaxTree_ExpressionRequireTrim_ShouldPass()
        {
            var tree = CheckSyntaxTree(" * * * * * * * ", "* * * * * * *");
            CheckHasAppropiateCountsOfSegments(tree);
            CheckLastOfSegmentsIsOfType<EndOfFileNode>(tree);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownTokenException))]
        public void CheckSyntaxTree_WithUnknownOperator_ShouldThrow()
        {
            var tree = CheckSyntaxTree("& * * * * * *");
        }

        [TestMethod]
        [ExpectedException(typeof(MismatchedSegmentsCountException))]
        public void CheckSyntaxTree_ExpressionIsToShort_ShouldThrow()
        {
            var tree = CheckSyntaxTree("* * *");
        }

        private void CheckHasAppropiateCountsOfSegments(RootComponentNode tree)
        {
            //Seven required segments + EndOfFile
            Assert.AreEqual(8, tree.Items.Count());
        }

        private void CheckLastOfSegmentsIsOfType<T>(RootComponentNode tree)
        {
            Assert.AreEqual(typeof(T), tree.Items.Last().GetType());
        }

        private RootComponentNode CheckSyntaxTree(string expression, string expectedOutputExpression)
        {
            var ast = expression.Parse();
            Assert.AreEqual(expectedOutputExpression, ast.ToString());
            return ast;
        }

        private RootComponentNode CheckSyntaxTree(string expression)
        {
            return CheckSyntaxTree(expression, expression);
        }
    }
}
