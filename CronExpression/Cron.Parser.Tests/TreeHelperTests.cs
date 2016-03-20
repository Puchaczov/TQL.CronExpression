using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cron.Parser.Helpers;
using Cron.Parser.Nodes;

namespace Cron.Parser.Tests
{
    [TestClass]
    public class TreeHelperTests
    {
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
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TreeHelper_FindByPath_IncorrectPath_ShouldFail()
        {
            var ast = "* * *".Parse();
            ast.FindByPath("0>10");
        }

        [TestMethod]
        public void TreeHelper_TraverseCheckSimpleCase_ShouldPass()
        {
            var ast = "* * * * * * *".Parse();
            int countToVisit = 16;
            int count = 0;
            ast.Traverse(f => count += 1);
            Assert.AreEqual(countToVisit, count);
        }

        [TestMethod]
        public void TreeHelper_TraverseCheckAllNodesVisited_ShouldPass()
        {
            var ast = "1-2/3 4 5#6 1/2 1-6,2,1-19/2,5#1 L W,LW".Parse();
            int countToVisit = 36;
            int count = 0;
            ast.Traverse(f => count += 1);
            Assert.AreEqual(countToVisit, count);
        }

        [TestMethod]
        public void TreeHelper_GetSpan_ShouldPass()
        {
            var ast = "1-2,5-6#4".Parse(false);
            var node = ast.FindBySpan(new Tokens.TextSpan(0, 3));
            Assert.AreEqual("1-2", node.ToString()); //should select whole segment
        }
    }
}
