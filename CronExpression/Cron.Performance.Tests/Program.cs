using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Performance.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();

            var timeline = new Cron.CronTimeline(true);
            var response = timeline.Convert(new Cron.Converter.CreateEvaluatorRequest("* * * * * * *", ConvertionRequest.CronMode.ModernDefinition, DateTime.Now, TimeZoneInfo.Local));
            var evaluator = response.Output;
            watch.Stop();
        }
    }
}
