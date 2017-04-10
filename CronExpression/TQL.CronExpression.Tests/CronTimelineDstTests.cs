using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TQL.CronExpression.Tests
{
    [TestClass]
    public class CronTimelineDstTests
    {
        [TestMethod]
        public void CodeGenerationVisitor_DaylightSavingTime_SpringTime_HoursResolution()
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var timeline = new CronTimeline()
                .Convert(new CreateEvaluatorRequest("0 0 * * * * 2015-2150", ConvertionRequest.CronMode.ModernDefinition, DateTime.Parse("25.03.2017 23:59:00"), timezone, timezone));
            
            var machine = timeline.Output;

            Assert.AreEqual(DateTimeOffset.Parse("26.03.2017 00:00:00 +01:00"), machine.NextFire());
            Assert.AreEqual(DateTimeOffset.Parse("26.03.2017 01:00:00 +01:00"), machine.NextFire());
            Assert.AreEqual(DateTimeOffset.Parse("26.03.2017 03:00:00 +02:00"), machine.NextFire());
            Assert.AreEqual(DateTimeOffset.Parse("26.03.2017 04:00:00 +02:00"), machine.NextFire());
            Assert.AreEqual(DateTimeOffset.Parse("26.03.2017 05:00:00 +02:00"), machine.NextFire());
        }

        [TestMethod]
        public void CodeGenerationVisitor_DaylightSavingTime_SpringTime_MinutesResolution()
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var timeline = new CronTimeline()
                .Convert(new CreateEvaluatorRequest("0 * * * * * 2015-2150", ConvertionRequest.CronMode.ModernDefinition, DateTime.Parse("26.03.2017 01:58:00"), timezone, timezone));

            var machine = timeline.Output;

            Assert.AreEqual(DateTimeOffset.Parse("26.03.2017 01:59:00 +01:00"), machine.NextFire());
            Assert.AreEqual(DateTimeOffset.Parse("26.03.2017 03:00:00 +02:00"), machine.NextFire());
            Assert.AreEqual(DateTimeOffset.Parse("26.03.2017 03:01:00 +02:00"), machine.NextFire());
            Assert.AreEqual(DateTimeOffset.Parse("26.03.2017 03:02:00 +02:00"), machine.NextFire());
            Assert.AreEqual(DateTimeOffset.Parse("26.03.2017 03:03:00 +02:00"), machine.NextFire());
        }

        [TestMethod]
        public void CodeGenerationVisitor_DaylightSavingTime_WinterTime_HoursResolution()
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var timeline = new CronTimeline()
                .Convert(new CreateEvaluatorRequest("0 0 * * * * 2015-2150", ConvertionRequest.CronMode.ModernDefinition, DateTime.Parse("28.10.2017 23:59:00"), timezone, timezone));

            var machine = timeline.Output;

            Assert.AreEqual(DateTimeOffset.Parse("29.10.2017 00:00:00 +02:00"), machine.NextFire());
            Assert.AreEqual(DateTimeOffset.Parse("29.10.2017 01:00:00 +02:00"), machine.NextFire());
            Assert.AreEqual(DateTimeOffset.Parse("29.10.2017 02:00:00 +02:00"), machine.NextFire());
            Assert.AreEqual(DateTimeOffset.Parse("29.10.2017 02:00:00 +01:00"), machine.NextFire());
            Assert.AreEqual(DateTimeOffset.Parse("29.10.2017 03:00:00 +01:00"), machine.NextFire());
        }
    }
}
