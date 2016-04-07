using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cron.Compilation;

namespace Cron.Compiler.Tests
{
    [TestClass]
    public class CompilerTests
    {

        [TestMethod]
        public void Compiler_CheckProduceAst_ShouldPass()
        {
            var compiler = new StandardCompiler();
            var options = new CompilationRequest.CompilationOptions {
                ProduceEndOfFileNode = true,
                ProduceYearIfMissing = true
            };

            var request = new CompilationRequest("* * * * * * *", options);

            Assert.IsNotNull(compiler.Compile(request));
        }
    }
}
