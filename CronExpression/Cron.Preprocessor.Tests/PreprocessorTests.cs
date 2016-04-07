using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cron.Tests
{
    [TestClass]
    public class PreprocessorTests
    {
        [TestMethod]
        public void Preprocessor_TrimInput_ShouldPass()
        {
            var preprocessor = new Preprocessor();
            Assert.AreEqual("* *", preprocessor.Execute(" * *   "));
        }

        [TestMethod]
        public void Preprocessor_LowercaseInput_ShouldPass()
        {
            var preprocessor = new Preprocessor();
            Assert.AreEqual("abc ccde", preprocessor.Execute("aBc CCde"));
        }

        [TestMethod]
        public void Preprocessor_ReplaceInputWhenNonStandardDefiniotionsOccured_ShouldReplace()
        {
            var preprocessor = new Preprocessor();
            Assert.AreEqual("0 0 0 1 1 * *", preprocessor.Execute("@YeArLy"));
            Assert.AreEqual("0 0 0 1 1 * *", preprocessor.Execute(" @anNually "));
            Assert.AreEqual("0 0 0 1 * * *", preprocessor.Execute("@monthly"));
            Assert.AreEqual("0 0 0 * * 0 *", preprocessor.Execute("@weekly"));
            Assert.AreEqual("0 0 0 * * * *", preprocessor.Execute("@daily"));
            Assert.AreEqual("0 0 * * * * *", preprocessor.Execute("@hourly"));
        }
    }
}
