using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TQL.CronExpression.Exceptions;
using TQL.CronExpression.TimelineEvaluator;
using TQL.CronExpression.Visitors;

namespace TQL.CronExpression.Converter.Tests
{
    [TestClass]
    public class ConverterTests
    {

        [TestMethod]
        public void Evaluator_CheckProduceAst_ShouldPass()
        {
            var compiler = new CronTimeline();
            var options = new ConvertionRequest.ConvertionOptions
            {
                ProduceEndOfFileNode = true,
                ProduceYearIfMissing = true
            };

            var request = new CreateEvaluatorRequest("* * * * * * *", options, DateTime.Now, TimeZoneInfo.Local);

            Assert.IsNotNull(compiler.Convert(request));
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
                .Convert(new CreateEvaluatorRequest("* *", ConvertionRequest.CronMode.ModernDefinition, DateTime.Now, TimeZoneInfo.Local));
        }

        [TestMethod]
        [ExpectedException(typeof(IncorrectCronExpressionException))]
        public void Evaluator_StandardDefinition_ShouldThrowAggregatedException()
        {
            new CronTimeline(true)
                .Convert(new CreateEvaluatorRequest("* *", ConvertionRequest.CronMode.StandardDefinition, DateTime.Now, TimeZoneInfo.Local));
        }

        [TestMethod]
        public void Evaluator_ModernDefinition_Issue01_ShouldReturnTwoErrorMessages()
        {
            var response = new CronTimeline(false)
                .Convert(new CreateEvaluatorRequest("0 0 0 29 2 * 2015-201", ConvertionRequest.CronMode.ModernDefinition, DateTime.Now, TimeZoneInfo.Local));

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Messages);
            Assert.IsNull(response.Output);
            Assert.AreEqual(2, response.Messages.Count);
        }

        [TestMethod]
        public void Evaluator_IncorrectExpressionProvidedWithMissingNode_ShouldContainCriticalErrorMessage()
        {
            var response = new CronTimeline(false)
                .Convert(new CreateEvaluatorRequest("0 0 0 29 2, * 2015-201", ConvertionRequest.CronMode.ModernDefinition, DateTime.Now, TimeZoneInfo.Local));
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Messages);
            Assert.IsNull(response.Output);
            Assert.AreEqual(4, response.Messages.Count);
            Assert.IsTrue(response.Messages.OfType<FatalVisitError>().Any());
        }

        [TestMethod]
        public void Validator_IncorrectExpressionProvidedWithMissingNode_ShouldContainErrorMessages()
        {
            var validator = new CronValidator(false)
                .Convert(new ConvertionRequest("0 0 0 29 2, * 2015-201", ConvertionRequest.CronMode.ModernDefinition));
            Assert.IsNotNull(validator);
            Assert.IsNotNull(validator.Messages);
            Assert.AreEqual(false, validator.Output);
            Assert.AreEqual(3, validator.Messages.Count);
        }

        [TestMethod]
        public void Evaluator_CheckEvaluatedValuesTakeIntoTargetTimeZone_ShouldPass()
        {
            var timeZoneReferenceTime = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time"); //+1
            var destinationZoneReferenceTime = TimeZoneInfo.FindSystemTimeZoneById("Mid-Atlantic Standard Time"); //-2
            var referenceTime = new DateTimeOffset(2015, 1, 1, 15, 0, 0, timeZoneReferenceTime.BaseUtcOffset);

            var response = new CronTimeline(false)
                .Convert(new CreateEvaluatorRequest("0 0 * * * * *", ConvertionRequest.CronMode.ModernDefinition, referenceTime, destinationZoneReferenceTime));

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual(0, response.Messages.Count);

            var evaluator = response.Output;

            Assert.IsNotNull(evaluator);
            Assert.AreEqual(new DateTimeOffset(2015, 1, 1, 13, 0, 0, new TimeSpan(-2, 0, 0)), evaluator.NextFire());
            Assert.AreEqual(new DateTimeOffset(2015, 1, 1, 14, 0, 0, new TimeSpan(-2, 0, 0)), evaluator.NextFire());
        }

        [TestMethod]
        public void Evaluator_CheckEvaluatedValues_ShouldPass()
        {
            new CronTimeline()
                .Convert(new CreateEvaluatorRequest("0 30 14 ? * 7L *", ConvertionRequest.CronMode.ModernDefinition, DateTimeOffset.Now, TimeZoneInfo.Local));
        }

        [TestMethod]
        public void Validator_CheckCanHandleUnbreakableSpace_ShouldPass()
        {
            new CronValidator()
                .Convert(new CreateEvaluatorRequest("0 0 2/4 8-14 * 2#5 *", ConvertionRequest.CronMode.ModernDefinition, DateTimeOffset.Now, TimeZoneInfo.Local));
        }

        [TestMethod]
        public void Validator_CheckWillProduceUninstantiableDateTime_ShouldFail()
        {
            var timeline = new CronTimeline()
                .Convert(new CreateEvaluatorRequest("0 0 0 L 2 * 2015-2150", ConvertionRequest.CronMode.ModernDefinition, DateTimeOffset.Now, TimeZoneInfo.Local));

            for (int i = 0; i < 5; ++i)
            {
                timeline.Output.NextFire();
            }
        }

        private static void CheckExpressionType(string input, ConvertionRequest.CronMode mode)
        {
            var compiler = new CronTimeline();
            var request = new CreateEvaluatorRequest(input, mode, DateTime.Now, TimeZoneInfo.Local);
            var response = compiler.Convert(request);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual(0, response.Messages.Count);
            Assert.IsNotNull(response.Output);
        }
    }
}
