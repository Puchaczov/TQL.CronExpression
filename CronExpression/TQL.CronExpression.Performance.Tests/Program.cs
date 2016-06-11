using System;
using System.Diagnostics;
using TQL.CronExpression.Converter;

namespace TQL.CronExpression.Performance.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();

            var timeline = new CronTimeline(true);
            var response = timeline.Convert(new CreateEvaluatorRequest("* * * * * * *", ConvertionRequest.CronMode.ModernDefinition, DateTime.Now, TimeZoneInfo.Local));
            var evaluator = response.Output;
            watch.Stop();
        }
    }
}
