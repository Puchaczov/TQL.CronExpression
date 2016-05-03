using Cron.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cron.Converter.Tests
{
    [TestClass]
    public class ConverterTests
    {

        [TestMethod]
        public void Evaluator_CheckProduceAst_ShouldPass()
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
        public void Evaluator_CheckProducedEvaluator_ShouldPass()
        {
            CheckExpressionType("* * * * *", ConvertionRequest.CronMode.StandardDefinition);
            CheckExpressionType("* * * * * * *", ConvertionRequest.CronMode.ModernDefinition);
        }

        [TestMethod]
        [ExpectedException(typeof(IncorrectCronExpressionException))]
        public void Evaluator_ModernDefinition_ShouldThrowAggregatedException()
        {
            new CronTimeline(true)
                .GetEvaluator(new ConvertionRequest("* *", ConvertionRequest.CronMode.ModernDefinition));
        }

        [TestMethod]
        [ExpectedException(typeof(IncorrectCronExpressionException))]
        public void Evaluator_StandardDefinition_ShouldThrowAggregatedException()
        {
            new CronTimeline(true)
                .GetEvaluator(new ConvertionRequest("* *", ConvertionRequest.CronMode.StandardDefinition));
        }

        [TestMethod]
        public void Evaluator_ModernDefinition_Issue01_ShouldReturnTwoErrorMessages()
        {
            var evaluator = new CronTimeline(false)
                .GetEvaluator(new ConvertionRequest("0 0 0 29 2 * 2015-201", ConvertionRequest.CronMode.ModernDefinition));

            Assert.IsNotNull(evaluator);
            Assert.IsNotNull(evaluator.Messages);
            Assert.IsNull(evaluator.Output);
            Assert.AreEqual(2, evaluator.Messages.Count);
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
