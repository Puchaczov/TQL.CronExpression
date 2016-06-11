using Microsoft.VisualStudio.TestTools.UnitTesting;
using TQL.Core.Tokens;

namespace TQL.CronExpression.Parser.Tests
{
    [TestClass]
    public class TextSpanTests
    {
        [TestMethod]
        public void CheckEquality_DifferentTextSpans_ShouldBeEqual()
        {
            var first = new TextSpan(1, 2);
            var second = new TextSpan(2, 5);

            Assert.AreNotEqual(first, second);
        }

        [TestMethod]
        public void CheckEquality_SameTextSpans_ShouldBeEqual()
        {
            var first = new TextSpan(1, 2);
            var second = new TextSpan(1, 2);

            Assert.AreEqual(first, second);
            Assert.AreEqual(first, first);
        }
    }
}
