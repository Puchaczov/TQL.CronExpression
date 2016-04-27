using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cron.Compilation;
using Cron.Compiler.Exceptions;

namespace Cron.Compiler.Tests
{
    [TestClass]
    public class CompilerTests
    {

        [TestMethod]
        public void Compiler_CheckProduceAst_ShouldPass()
        {
            var compiler = new CronTimeLineEvaluationCompiler();
            var options = new CompilationRequest.CompilationOptions {
                ProduceEndOfFileNode = true,
                ProduceYearIfMissing = true
            };

            var request = new CompilationRequest("* * * * * * *", options);

            Assert.IsNotNull(compiler.Compile(request));
        }

        [TestMethod]
        public void Compiler_CheckProducedEvaluator_ShouldPass()
        {
            CheckExpressionType("* * * * *", CompilationRequest.CronMode.StandardDefinition);
            CheckExpressionType("* * * * * * *", CompilationRequest.CronMode.ModernDefinition);
        }

        [TestMethod]
        [ExpectedException(typeof(IncorrectCronExpressionException))]
        public void Compiler_ModernDefinition_ShouldThrowAggregatedException()
        {
            new CronTimeLineEvaluationCompiler(true)
                .Compile(new CompilationRequest("* *", CompilationRequest.CronMode.ModernDefinition));
        }

        [TestMethod]
        [ExpectedException(typeof(IncorrectCronExpressionException))]
        public void Compiler_StandardDefinition_ShouldThrowAggregatedException()
        {
            new CronTimeLineEvaluationCompiler(true)
                .Compile(new CompilationRequest("* *", CompilationRequest.CronMode.StandardDefinition));
        }

        private static void CheckExpressionType(string input, CompilationRequest.CronMode mode)
        {
            var compiler = new CronTimeLineEvaluationCompiler();
            var request = new CompilationRequest(input, mode);
            var response = compiler.Compile(request);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual(0, response.Messages.Count);
            Assert.IsNotNull(response.Output);
        }
    }
}
