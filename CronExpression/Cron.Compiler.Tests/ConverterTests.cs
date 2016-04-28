using Cron.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cron.Converter.Tests
{
    [TestClass]
    public class ConverterTests
    {

        [TestMethod]
        public void Compiler_CheckProduceAst_ShouldPass()
        {
            var compiler = new CronTimeline();
            var options = new ConvertionRequest.ConvertionOptions {
                ProduceEndOfFileNode = true,
                ProduceYearIfMissing = true
            };

            var request = new ConvertionRequest("* * * * * * *", options);

            Assert.IsNotNull(compiler.GetEvaluator(request));
        }

        [TestMethod]
        public void Compiler_CheckProducedEvaluator_ShouldPass()
        {
            CheckExpressionType("* * * * *", ConvertionRequest.CronMode.StandardDefinition);
            CheckExpressionType("* * * * * * *", ConvertionRequest.CronMode.ModernDefinition);
        }

        [TestMethod]
        [ExpectedException(typeof(IncorrectCronExpressionException))]
        public void Compiler_ModernDefinition_ShouldThrowAggregatedException()
        {
            new CronTimeline(true)
                .GetEvaluator(new ConvertionRequest("* *", ConvertionRequest.CronMode.ModernDefinition));
        }

        [TestMethod]
        [ExpectedException(typeof(IncorrectCronExpressionException))]
        public void Compiler_StandardDefinition_ShouldThrowAggregatedException()
        {
            new CronTimeline(true)
                .GetEvaluator(new ConvertionRequest("* *", ConvertionRequest.CronMode.StandardDefinition));
        }

        private static void CheckExpressionType(string input, ConvertionRequest.CronMode mode)
        {
            var compiler = new CronTimeline();
            var request = new ConvertionRequest(input, mode);
            var response = compiler.GetEvaluator(request);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual(0, response.Messages.Count);
            Assert.IsNotNull(response.Output);
        }
    }
}
