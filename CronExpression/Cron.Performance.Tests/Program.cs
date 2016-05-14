using Cron.Converter;
using System;
using System.Diagnostics;

namespace Cron.Performance.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();

            var timeline = new CronTimeline(true);
            var response = timeline.Convert(new Cron.Converter.CreateEvaluatorRequest("* * * * * * *", ConvertionRequest.CronMode.ModernDefinition, DateTime.Now, TimeZoneInfo.Local));
            var evaluator = response.Output;
            watch.Stop();
        }
    }
}
