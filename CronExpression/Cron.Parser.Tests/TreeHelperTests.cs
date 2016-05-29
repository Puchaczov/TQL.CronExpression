using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cron.Parser.Helpers;
using Cron.Parser.Nodes;
using System.Linq;
using TQL.Core.Tokens;

namespace Cron.Parser.Tests
{
    [TestClass]
    public class TreeHelperTests
    {

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TreeHelper_FindByPath_IncorrectPath_ShouldFail()
        {
            var ast = "* * *".Parse();
            ast.FindByPath("0>10");
        }
        [TestMethod]
        public void TreeHelper_FindByPath_ShouldPass()
        {
            var ast = "1-2/3 4 5#6 1/2 1-6,2,1-19/2,5#1 L W,LW".Parse();

            Assert.AreEqual("1-2/3", ast.FindByPath("0>0").ToString());
            Assert.AreEqual(typeof(IncrementByNode), ast.FindByPath("0>0>0").GetType());
            Assert.AreEqual("1-2", ast.FindByPath("0>0>0>0").ToString());
            Assert.AreEqual("1", ast.FindByPath("0>0>0>0>0").ToString());
            Assert.AreEqual("2", ast.FindByPath("0>0>0>0>1").ToString());
            Assert.AreEqual("3", ast.FindByPath("0>0>0>1").ToString());

            Assert.AreEqual("4", ast.FindByPath("0>1>0").ToString());

            Assert.AreEqual("5#6", ast.FindByPath("0>2>0").ToString());
            Assert.AreEqual("5", ast.FindByPath("0>2>0>0").ToString());
            Assert.AreEqual("6", ast.FindByPath("0>2>0>1").ToString());

            Assert.AreEqual(typeof(RangeNode), ast.FindByPath("0>4>0").GetType());
            Assert.AreEqual("2", ast.FindByPath("0>4>1").ToString());
            Assert.AreEqual(typeof(IncrementByNode), ast.FindByPath("0>4>2").GetType()); //1-19/2
            Assert.AreEqual(typeof(HashNode), ast.FindByPath("0>4>3").GetType()); //5#1
        }

        [TestMethod]
        public void TreeHelper_GetSpanByCaret_ShouldPass()
        {
            var ast = "1-2,5-6#4".Parse(false);
            var node = ast.FindBySpan(new TextSpan(0, 3));
            Assert.AreEqual("1-2", node.ToString()); //should select whole segment
            node = ast.FindBySpan(0);
            Assert.AreEqual("1", node.ToString());
            node = ast.FindBySpan(1);
            Assert.AreEqual("1-2", node.ToString());
            node = ast.FindBySpan(2);
            Assert.AreEqual("2", node.ToString());
            node = ast.FindBySpan(3);
            Assert.AreEqual("1-2,5-6#4", node.ToString());
            node = ast.FindBySpan(4);
            Assert.AreEqual("5", node.ToString());
            node = ast.FindBySpan(5);
            Assert.AreEqual("5-6", node.ToString());
            node = ast.FindBySpan(6);
            Assert.AreEqual("6", node.ToString());
            node = ast.FindBySpan(7);
            Assert.AreEqual("5-6#4", node.ToString());
            node = ast.FindBySpan(8);
            Assert.AreEqual("4", node.ToString());
        }

        [TestMethod]
        public void TreeHelper_TraverseCheckAllNodesVisited_ShouldPass()
        {
            var ast = "1-2/3 4 5#6 1/2 1-6,2,1-19/2,5#1 L W,LW".Parse();
            const int countToVisit = 36;
            var count = 0;
            ast.Traverse(f => count += 1);
            Assert.AreEqual(countToVisit, count);
        }

        [TestMethod]
        public void TreeHelper_TraverseCheckSimpleCase_ShouldPass()
        {
            var ast = "* * * * * * *".Parse();
            const int countToVisit = 16;
            var count = 0;
            ast.Traverse(f => count += 1);
            Assert.AreEqual(countToVisit, count);
        }

        [TestMethod]
        public void TreeHelper_GetSiblingsOfRoot_ShouldReturnNull()
        {
            var ast = @"* * * * * * *".Parse();
            Assert.IsNull(ast.Siblings(ast));
        }

        [TestMethod]
        public void TreeHelper_GetSiblingsOfSegment_ShouldReturnSegments()
        {
            var ast = @"* * * * * * *".Parse();
            var segments = ast.Siblings(ast.Segments[1]);
            Assert.IsNotNull(segments);
            Assert.AreEqual(7, segments.Length);
            Assert.AreEqual(0, segments.Count(f => ReferenceEquals(f, ast.Segments[1])));
        }
    }
}
