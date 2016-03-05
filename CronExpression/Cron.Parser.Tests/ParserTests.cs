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
        public void CheckSyntaxTree_WithQuestionMark_ShouldPass()
        {
            var tree = CheckSyntaxTree("* * * ? * * *");
            CheckHasAppropiateCountsOfSegments(tree);
            CheckLastOfSegmentsIsOfType<EndOfFileNode>(tree);
        }

        [TestMethod]
        public void CheckSyntaxTree_WithWeekDay_ShouldPass()
        {
            var tree = CheckSyntaxTree("MON TUE WED * * * *");
            CheckHasAppropiateCountsOfSegments(tree);
            CheckLastOfSegmentsIsOfType<EndOfFileNode>(tree);
        }

        [TestMethod]
        public void CheckSyntaxTree_ExpressionIsTooShort_ShouldPass()
        {
            var tree = CheckSyntaxTree("* * *", false);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownSegmentException))]
        public void CheckSyntaxTree_ExpressionIsTooLong_ShouldFail()
        {
            var tree = CheckSyntaxTree("* * * * * * * * * *");
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicatedExpressionException))]
        public void CheckSyntaxTree_DuplicatedComma_After_ShouldThrow()
        {
            CheckSyntaxTree("1,, * * * * * *");
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedOperatorException))]
        public void CheckSyntaxTree_DuplicatedComma_Middle_ShouldThrow()
        {
            CheckSyntaxTree(",1, * * * * * *");
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedOperatorException))]
        public void CheckSyntaxTree_DuplicatedComma_Before_ShouldThrow()
        {
            CheckSyntaxTree(",,1 * * * * * *");
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedOperatorException))]
        public void CheckSyntaxTree_UnexpectedCommaBeforeInteger_ShouldThrow()
        {
            CheckSyntaxTree(",1 * * * * * *");
        }

        [TestMethod]
        public void CheckSyntaxTree_IrregularWhitespaceBetweenSegments_ShouldPass()
        {
            CheckSyntaxTree("*  *   *   * *        *             *", "* * * * * * *");
        }

        [TestMethod]
        public void CheckSyntaxTree_IncFollowedByRange_ShouldPass()
        {
            CheckSyntaxTree("* * 1-5/10 * * * *");
        }

        [TestMethod]
        public void CheckSyntaxTree_WithNumericPrecededLNode_ShouldPass()
        {
            CheckSyntaxTree("* 50L * * * * *");
        }

        [TestMethod]
        public void CheckSyntaxTree_WithNumericPrecededWNode_ShouldPass()
        {
            CheckSyntaxTree("* 50W * * * * *");
        }

        [TestMethod]
        public void CheckSyntaxTree_WithNumericPrecededLWNode_ShouldPass()
        {
            CheckSyntaxTree("* 50LW * * * * *");
        }

        [TestMethod]
        public void CheckSyntaxTree_WithHashNode_ShouldPass()
        {
            CheckSyntaxTree("* 3#5 * * * * *");
        }

        private void CheckHasAppropiateCountsOfSegments(RootComponentNode tree)
        {
            //Seven required segments + EndOfFile
            Assert.AreEqual(8, tree.Desecendants.Count());
        }

        private void CheckLastOfSegmentsIsOfType<T>(RootComponentNode tree)
        {
            Assert.AreEqual(typeof(T), tree.Desecendants.Last().GetType());
        }

        private RootComponentNode CheckSyntaxTree(string expression, string expectedOutputExpression, bool produceMissingYearSegment = true)
        {
            var ast = expression.Parse(produceMissingYearSegment);
            Assert.AreEqual(expectedOutputExpression, ast.ToString());
            return ast;
        }

        private RootComponentNode CheckSyntaxTree(string expression, bool produceMissingYearSegment = true)
        {
            return CheckSyntaxTree(expression, expression, produceMissingYearSegment);
        }
    }
}
