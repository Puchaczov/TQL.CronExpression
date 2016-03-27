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
            StandardCompiler compiler = new StandardCompiler();
            CompilationRequest.CompilationOptions options = new CompilationRequest.CompilationOptions() {
                ProduceEndOfFileNode = true,
                ProduceYearIfMissing = true
            };

            CompilationRequest request = new CompilationRequest("* * * * * * *", options);

            Assert.IsNotNull(compiler.Compile(request));
        }
    }
}
